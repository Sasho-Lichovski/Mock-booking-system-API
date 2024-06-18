using Services.Enums;
using Services.Models.Book;

namespace Services.Interfaces
{
    public interface IBookService
    {
        BookingStatusEnum CheckStatus(string bookingCode);
        BookRes CreateBooking(BookReq request);
    }
}
