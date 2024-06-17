using System.ComponentModel.DataAnnotations;

namespace Services.Models.Search
{
    public class SearchModel
    {
        public string DepartureAirport { get; set; }
        [Required]
        public string ArrivalAirport { get; set; }
        [Required]
        public DateTime? DateFrom { get; set; }
        [Required]
        public DateTime? DateTo { get; set; }
    }
}
