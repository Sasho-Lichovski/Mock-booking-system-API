using Repository.Interfaces;
using Services.Interfaces;
using Services.Models.Book;
using Utils.Constants;

namespace Services.Services
{
    public class BookService : IBookService
    {
        private readonly ICacheRepository cacheRepository;

        public BookService(ICacheRepository cacheRepository)
        {
            this.cacheRepository = cacheRepository;
        }

        public void CreateBooking(BookReq request)
        {
            var booking = new Booking()
            {
                BookingCode = request.OptionCode,
                SleepingTime = GetSleepingTime(30, 60)
            };

            cacheRepository.Set(Book.Booking, booking.BookingCode, booking);
        }

        private int GetSleepingTime(int small, int big) 
        {
            Random rng = new Random();
            return rng.Next(small, big);
        }
    }
}
