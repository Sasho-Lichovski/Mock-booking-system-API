namespace Services.Models.Search
{
    public class Option
    {
        public string OptionCode { get; set; }

        public string HotelCode { get; set; }
        public string HotelName { get; set; }
        public string City { get; set; }

        public string FlightCode { get; set; }
        public string FlightNumber { get; internal set; }
        public string ArrivalAirpot { get; set; }

        public double Price { get; set; }
        public DateTime DateFrom { get; set; }
    }
}
