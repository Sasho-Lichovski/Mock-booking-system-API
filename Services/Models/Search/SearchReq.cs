namespace Services.Models.Search
{
    public class SearchReq
    {
        public string DepartureAirport { get; set; }
        public string Destination { get; set; }
        public DateTime? DateFrom { get; set; }
        public DateTime? DateTo { get; set; }
    }
}
