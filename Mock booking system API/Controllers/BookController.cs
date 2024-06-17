using Microsoft.AspNetCore.Mvc;
using Services.Interfaces;
using Services.Models.Book;

namespace Mock_booking_system_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BookController : ControllerBase
    {
        private readonly IBookService bookService;

        public BookController(IBookService bookService)
        {
            this.bookService = bookService;
        }

        [HttpPost]
        public  IActionResult Book([FromBody] BookReq request)
        {
            try
            {
                bookService.CreateBooking(request);
            }
            catch (Exception ex)
            {
            }

            return Ok("Pending");
        }
    }
}
