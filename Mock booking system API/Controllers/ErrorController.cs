using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Services.Interfaces;
using Services.Models.Error;

namespace Mock_booking_system_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class ErrorController : ControllerBase
    {
        private readonly IErrorService errorService;

        public ErrorController(IErrorService errorService)
        {
            this.errorService = errorService;
        }

        [HttpGet]
        [Route("CheckErrors")]
        public IActionResult CheckErrors(DateTime dateFrom)
        {
            List<ErrorModel> errors = errorService.GetErrors(dateFrom);
            return Ok(errors);
        }
    }
}
