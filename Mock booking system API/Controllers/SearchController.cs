using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Services.Interfaces;
using Services.Models.Search;

namespace Mock_booking_system_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class SearchController : ControllerBase
    {
        private readonly ISearchService searchService;

        public SearchController(ISearchService searchService)
        {
            this.searchService = searchService;
        }

        [HttpPost]
        [Route("Search")]
        public async Task<IActionResult> Search([FromBody] SearchReq request)
        {
            if (string.IsNullOrWhiteSpace(request.Destination))
                return Ok("Please provide arrival airpot");

            if (request.DateFrom == null)
                return Ok("Please provide date from");

            if (request.DateTo == null)
                return Ok("Please provide date to");

            SearchRes response = new SearchRes();
            try
            {
                response = await searchService.Search(request);
            }
            catch (Exception ex)
            {
            }

            if (response.Options == null || response.Options.Count == 0)
                return Ok("No hotels found");

            return Ok(response);
        }
    }
}
