using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Services.Enums;
using Services.Interfaces;

namespace Mock_booking_system_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class StatusController : ControllerBase
    {
        private readonly IBookService bookService;

        public StatusController(IBookService bookService)
        {
            this.bookService = bookService;
        }

        [HttpGet]
        [Route("CheckStatus")]
        public ActionResult CheckStatus(string bookingCode)
        {
            BookingStatusEnum status = BookingStatusEnum.Pending;
            try
            {
                status = bookService.CheckStatus(bookingCode);
            }
            catch (Exception ex)
            {
            }

            return Ok(status.ToFriendlyName());
        }
    }
}
