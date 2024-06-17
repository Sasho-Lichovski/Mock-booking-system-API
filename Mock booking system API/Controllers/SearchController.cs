using Microsoft.AspNetCore.Mvc;
using Services.Interfaces;
using Services.Models.Search;

namespace Mock_booking_system_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SearchController : ControllerBase
    {
        private readonly ISearchService searchService;

        public SearchController(ISearchService searchService)
        {
            this.searchService = searchService;
        }

        [HttpPost]
        [Route("Search")]
        public IActionResult Search([FromBody] SearchModel model)
        {
            if (string.IsNullOrWhiteSpace(model.ArrivalAirport))
                return Ok("Please provide arrival airpot");

            ResponseModel response = new ResponseModel();
            try
            {
                response = searchService.Search(model);
            }
            catch (Exception ex)
            {
            }

            if (response.Hotels == null || response.Hotels.Count == 0)
                return Ok("No hotels found");

            if (response.FlightsAndHotels == null)
                return Ok(response.Hotels);
            else
                return Ok(response.FlightsAndHotels);
        }
    }
}
