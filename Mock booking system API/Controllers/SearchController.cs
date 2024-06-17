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
        public async Task<IActionResult> Search([FromBody] SearchModel model)
        {
            if (string.IsNullOrWhiteSpace(model.ArrivalAirport))
                return Ok("Please provide arrival airpot");

            if (model.DateFrom == null)
                return Ok("Please provide date from");

            if (model.DateTo == null)
                return Ok("Please provide date to");

            ResponseModel response = new ResponseModel();
            try
            {
                response = await searchService.Search(model);
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
