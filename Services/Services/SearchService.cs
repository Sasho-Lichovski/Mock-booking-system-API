using Newtonsoft.Json;
using Repository.Interfaces;
using RestSharp;
using Services.Interfaces;
using Services.Models;
using Services.Models.Search;
using Utils.Constants;
using Utils.Helpers;

namespace Services.Services
{
    public class SearchService : ISearchService
    {
        private readonly ICacheRepository cacheRepository;
        private string searchHotelsUrl = "https://tripx-test-functions.azurewebsites.net/api/SearchHotels?destinationCode={code}";
        private string searchFlights = "https://tripx-test-functions.azurewebsites.net/api/SearchFlights?departureAirport={dptCode}&arrivalAirport={arrCode}";

        public SearchService(ICacheRepository cacheRepository)
        {
            this.cacheRepository = cacheRepository;
        }

        private List<Option> CreateHotelOptions(List<Hotel> hotels, DateTime dateFrom, bool isNew = false)
        {
            var options = new List<Option>();
            foreach (var hotel in hotels)
            {
                var option = new Option
                {
                    HotelCode = hotel.HotelCode.ToString(),
                    HotelName = hotel.HotelName,
                    City = hotel.City,
                    ArrivalAirpot = hotel.DestinationCode,
                    FlightCode = Message.NotApplicable,
                    FlightNumber = Message.NotApplicable,
                    OptionCode = isNew ? Calculate.GetOptionCode(6) : hotel.OptionCode,
                    Price = Calculate.GetRandomNumberBetween(100, 300),
                    DateFrom = dateFrom
                };
                hotel.OptionCode = option.OptionCode;
                options.Add(option);
            }
            return options;
        }

        public async Task<SearchRes> Search(SearchReq model)
        {
            var response = new SearchRes { Options = new List<Option>() };

            var hotels = new List<Hotel>();

            var cachedHotels = cacheRepository.Get(SearchTypes.Hotels, model.Destination);
            if (string.IsNullOrEmpty(cachedHotels) || cachedHotels == null)
            {
                var jsonString = await GetHotels(model.Destination);
                if (!string.IsNullOrEmpty(jsonString))
                {
                    hotels = JsonConvert.DeserializeObject<List<Hotel>>(jsonString);
                    response.Options = CreateHotelOptions(hotels, model.DateFrom.Value, true);
                    cacheRepository.Set(SearchTypes.Hotels, model.Destination, hotels);
                }
            }
            else
            {
                hotels = JsonConvert.DeserializeObject<List<Hotel>>(cachedHotels);
                response.Options = CreateHotelOptions(hotels, model.DateFrom.Value);
            }

            if (!hotels.Any())
                return response;

            if (Calculate.IsLastMinuteCall(model.DateFrom.Value, SearchTypes.LastMinuteDays))
                return response;

            if (!string.IsNullOrWhiteSpace(model.DepartureAirport))
            {
                var jsonString = cacheRepository.Get(SearchTypes.Combined, $"{model.DepartureAirport}-{model.Destination}");

                if (string.IsNullOrEmpty(jsonString) || jsonString == null)
                    jsonString = await GetFlights(model.DepartureAirport, model.Destination);

                if (string.IsNullOrEmpty(jsonString))
                    return response;

                var flights = JsonConvert.DeserializeObject<List<Flight>>(jsonString);
                cacheRepository.Set(SearchTypes.Combined, $"{model.DepartureAirport}-{model.Destination}", flights);

                var hotelMatches = response.Options.Where(x => flights.Select(x => x.ArrivalAirport).Contains(x.ArrivalAirpot));
                foreach (var option in hotelMatches)
                {
                    var flight = flights.FirstOrDefault(x => x.ArrivalAirport == option.ArrivalAirpot);
                    if (flight == null) continue;

                    option.FlightCode = flight.FlightCode.ToString();
                    option.FlightNumber = flight.FlightNumber;
                    option.Price = Calculate.GetRandomNumberBetween(300, 500);
                }
            }

            return response;
        }

        private async Task<string> GetHotels(string code)
        {
            var client = new RestClient(searchHotelsUrl.Replace("{code}", code));
            var request = new RestRequest();
            request.Method = Method.Get;
            var response = await client.ExecuteAsync(request);
            if (response.IsSuccessful && response.StatusCode == System.Net.HttpStatusCode.OK && !string.IsNullOrEmpty(response.Content) && response.Content != "[]")
                return response.Content;

            return string.Empty;
        }

        private async Task<string> GetFlights(string dptCode, string arrCode)
        {
            var client = new RestClient(searchFlights.Replace("{dptCode}", dptCode).Replace("{arrCode}", arrCode));
            var request = new RestRequest();
            request.Method = Method.Get;
            var response = await client.ExecuteAsync(request);
            if (response.IsSuccessful && response.StatusCode == System.Net.HttpStatusCode.OK && !string.IsNullOrEmpty(response.Content) && response.Content != "[]")
                return response.Content;

            return string.Empty;
        }
    }
}
