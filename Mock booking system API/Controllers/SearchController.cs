using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Services.Interfaces;
using Services.Models.Error;
using Services.Models.Search;

namespace Mock_booking_system_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class SearchController : ControllerBase
    {
        private readonly ISearchService searchService;
        private readonly IErrorService errorService;

        public SearchController(ISearchService searchService, IErrorService errorService)
        {
            this.searchService = searchService;
            this.errorService = errorService;
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

                // test purpose
                //throw new Exception(request.ToString());
            }
            catch (Exception ex)
            {
                errorService.LogError(new ErrorModel
                {
                    Controller = "Search",
                    Action = "Search",
                    ErrorMessage = ex.Message,
                    StackTrace = ex.StackTrace,
                    AdditionalDetails = JsonConvert.SerializeObject(request),
                    LogTime = DateTime.Now
                });
                throw;
            }

            if (response.Options == null || response.Options.Count == 0)
                return Ok("No hotels found");

            return Ok(response);
        }
    }
}
