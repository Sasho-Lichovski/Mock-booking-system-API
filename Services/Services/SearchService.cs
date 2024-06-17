using Newtonsoft.Json;
using Repository.Interfaces;
using RestSharp;
using Services.Interfaces;
using Services.Models;
using Services.Models.Search;
using Utils.Constants;

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

        public async Task<ResponseModel> Search(SearchModel model)
        {
            var searchResponse = new ResponseModel();

            var cachedHotels = cacheRepository.Get(SearchTypes.Hotels, model.ArrivalAirport);
            if (string.IsNullOrEmpty(cachedHotels) || cachedHotels == null)
            {
                var jsonString = await GetHotels(model.ArrivalAirport);
                if (!string.IsNullOrEmpty(jsonString))
                {
                    var hotels = JsonConvert.DeserializeObject<List<Hotel>>(jsonString);
                    cacheRepository.Set(SearchTypes.Hotels, model.ArrivalAirport, hotels);
                    searchResponse.Hotels = hotels;
                }
            }
            else
            {
                searchResponse.Hotels = JsonConvert.DeserializeObject<List<Hotel>>(cachedHotels);
            }

            var isLastMinuteCall = IsLastMinuteCall(model.DateFrom.Value, 45);
            if (isLastMinuteCall)
                return searchResponse;

            if (!string.IsNullOrWhiteSpace(model.DepartureAirport))
            {
                var cachedFlights = cacheRepository.Get(SearchTypes.Combined, $"{model.DepartureAirport}-{model.ArrivalAirport}");
                if (string.IsNullOrEmpty(cachedFlights) || cachedFlights == null)
                {
                    var jsonString = await GetFlights(model.DepartureAirport, model.ArrivalAirport);
                    cachedFlights = jsonString;
                }

                if (!string.IsNullOrEmpty(cachedFlights))
                {
                    var flights = JsonConvert.DeserializeObject<List<Flight>>(cachedFlights);
                    cacheRepository.Set(SearchTypes.Combined, $"{model.DepartureAirport}-{model.ArrivalAirport}", flights);
                    searchResponse.FlightsAndHotels = new List<FlightAndHotel>();
                    foreach (var flight in flights)
                    {
                        var hotels = searchResponse.Hotels.Where(x => x.DestinationCode == flight.ArrivalAirport);
                        if (hotels.Any())
                        {
                            var flightsAndHotels = new FlightAndHotel()
                            {
                                ArrivalAirport = flight.ArrivalAirport,
                                DepartureAirport = flight.DepartureAirport,
                                FlightCode = flight.FlightCode,
                                FlightNumber = flight.FlightNumber,
                                DestinationHotels = new List<Hotel>()
                            };
                            foreach (var hotel in hotels)
                                flightsAndHotels.DestinationHotels.Add(hotel);

                            searchResponse.FlightsAndHotels.Add(flightsAndHotels);
                        }
                    }
                }
            }

            return searchResponse;
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

        public bool IsLastMinuteCall(DateTime dateFrom, int daysCount)
        {
            DateTime today = DateTime.Now;
            DateTime targetDate = today.AddDays(daysCount);

            return dateFrom > today && dateFrom <= targetDate;
        }
    }
}
