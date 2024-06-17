namespace Services.Models
{
    public class FlightAndHotel
    {
        public int FlightCode { get; set; }
        public string FlightNumber { get; set; }
        public string DepartureAirport { get; set; }
        public string ArrivalAirport { get; set; }

        public List<Hotel> DestinationHotels { get; set; }
    }
}
