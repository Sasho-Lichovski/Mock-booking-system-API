using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Services.Interfaces;
using Services.Models.Book;
using Services.Models.Error;

namespace Mock_booking_system_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class BookController : ControllerBase
    {
        private readonly IBookService bookService;
        private readonly IErrorService errorService;

        public BookController(IBookService bookService, IErrorService errorService)
        {
            this.bookService = bookService;
            this.errorService = errorService;
        }

        [HttpPost]
        [Route("Book")]
        public  IActionResult Book([FromBody] BookReq request)
        {
            BookRes response = new BookRes();
            try
            {
                response = bookService.CreateBooking(request);

                // test purpose
                //throw new Exception("Book error");
            }
            catch (Exception ex)
            {
                errorService.LogError(new ErrorModel
                {
                    Controller = "Book",
                    Action = "Book",
                    ErrorMessage = ex.Message,
                    StackTrace = ex.StackTrace,
                    AdditionalDetails = JsonConvert.SerializeObject(request),
                    LogTime = DateTime.Now
                });
                throw;
            }
            
            if (!string.IsNullOrEmpty(response.Message))
                return Ok(response.Message);

            return Ok(new { response.BookingTime, response.BookingCode });
        }
    }
}
