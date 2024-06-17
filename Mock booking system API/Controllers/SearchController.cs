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
        public async Task<IActionResult> Search([FromBody] SearchReq model)
        {
            if (string.IsNullOrWhiteSpace(model.Destination))
                return Ok("Please provide arrival airpot");

            if (model.DateFrom == null)
                return Ok("Please provide date from");

            if (model.DateTo == null)
                return Ok("Please provide date to");

            ResponseReq response = new ResponseReq();
            try
            {
                response = await searchService.Search(model);
            }
            catch (Exception ex)
            {
            }

            if (response.Options == null)
                return Ok("No hotels found");

            return Ok(response);
        }
    }
}
