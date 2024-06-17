using Services.Models.Book;

namespace Services.Interfaces
{
    public interface IBookService
    {
        void CreateBooking(BookReq request);
    }
}
