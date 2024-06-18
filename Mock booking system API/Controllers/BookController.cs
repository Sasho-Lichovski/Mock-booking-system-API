using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Services.Interfaces;
using Services.Models.Book;

namespace Mock_booking_system_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class BookController : ControllerBase
    {
        private readonly IBookService bookService;

        public BookController(IBookService bookService)
        {
            this.bookService = bookService;
        }

        [HttpPost]
        [Route("Book")]
        public  IActionResult Book([FromBody] BookReq request)
        {
            BookRes response = new BookRes();
            try
            {
                response = bookService.CreateBooking(request);
            }
            catch (Exception ex)
            {
            }
            
            if (!string.IsNullOrEmpty(response.Message))
                return Ok(response.Message);

            return Ok(new { response.BookingTime, response.BookingCode });
        }
    }
}
