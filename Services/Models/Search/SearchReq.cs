using System.ComponentModel.DataAnnotations;

namespace Services.Models.Search
{
    public class SearchReq
    {
        public string DepartureAirport { get; set; }
        [Required]
        public string Destination { get; set; }
        [Required]
        public DateTime? DateFrom { get; set; }
        [Required]
        public DateTime? DateTo { get; set; }
    }
}
