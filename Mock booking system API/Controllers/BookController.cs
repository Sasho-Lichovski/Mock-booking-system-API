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
            var validationMessage = ValidateRequest(request);
            if (!string.IsNullOrEmpty(validationMessage))
                return Ok(validationMessage);

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

        private string ValidateRequest(BookReq request)
        {
            if (string.IsNullOrEmpty(request.OptionCode))
                return "Please provide option code";

            if (request.SearchReq == null)
                return "Please provide search request";

            if (string.IsNullOrWhiteSpace(request.SearchReq.Destination))
                return "Please provide destination";

            if (request.SearchReq.DateFrom == null)
                return "Please provide date from";

            if (request.SearchReq.DateTo == null)
                return "Please provide date to";

            return string.Empty;
        }

    }
}
