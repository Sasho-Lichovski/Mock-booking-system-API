using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Services.Enums;
using Services.Interfaces;
using Services.Models.Error;

namespace Mock_booking_system_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class StatusController : ControllerBase
    {
        private readonly IBookService bookService;
        private readonly IErrorService errorService;

        public StatusController(IBookService bookService, IErrorService errorService)
        {
            this.bookService = bookService;
            this.errorService = errorService;
        }

        [HttpGet]
        [Route("CheckStatus")]
        public ActionResult CheckStatus(string bookingCode)
        {
            if (string.IsNullOrEmpty(bookingCode))
                return Ok("Please provide booking code");

            BookingStatusEnum status = BookingStatusEnum.Pending;
            try
            {
                status = bookService.CheckStatus(bookingCode);
            }
            catch (Exception ex)
            {
                errorService.LogError(new ErrorModel
                {
                    Controller = "Status",
                    Action = "CheckStatus",
                    ErrorMessage = ex.Message,
                    StackTrace = ex.StackTrace,
                    AdditionalDetails = bookingCode,
                    LogTime = DateTime.Now
                });
                throw;
            }

            return Ok(status.ToFriendlyName());
        }
    }
}
