using Newtonsoft.Json;
using Repository.Interfaces;
using Services.Enums;
using Services.Interfaces;
using Services.Models;
using Services.Models.Book;
using Utils.Constants;
using Utils.Helpers;

namespace Services.Services
{
    public class BookService : IBookService
    {
        private readonly ICacheRepository cacheRepository;

        public BookService(ICacheRepository cacheRepository)
        {
            this.cacheRepository = cacheRepository;
        }

        public BookingStatusEnum CheckStatus(string bookingCode)
        {
            var jsonString = cacheRepository.Get(Book.Booking, bookingCode);
            if (string.IsNullOrEmpty(jsonString))
                return BookingStatusEnum.Failed;

            var booking = JsonConvert.DeserializeObject<Booking>(jsonString);
            if (booking == null)
                return BookingStatusEnum.Failed;

            var isLastMinute = Calculate.IsLastMinuteCall(booking.SearchReq.DateFrom.Value, SearchTypes.LastMinuteDays);
            var isSleepingTimePassed = booking.BookingTime.AddSeconds(booking.SleepingTime) < DateTime.Now;

            if (isLastMinute && isSleepingTimePassed)
                return BookingStatusEnum.Failed;
            else if (!isLastMinute && isSleepingTimePassed)
                return BookingStatusEnum.Success;

            return BookingStatusEnum.Pending;
        }

        public BookRes CreateBooking(BookReq request)
        {
            var jsonString = cacheRepository.Get(Book.Booking, request.OptionCode);
            if (!string.IsNullOrWhiteSpace(jsonString))
            {
                return new BookRes
                {
                    Message = Book.BookingExists
                };
            }

            jsonString = cacheRepository.Get(SearchTypes.Hotels, request.SearchReq.Destination);
            if (string.IsNullOrEmpty(jsonString))
            {
                return new BookRes
                {
                    Message = Book.BookingCodeDoesNotExist
                };
            }
            else
            {
                var hotels = JsonConvert.DeserializeObject<List<Hotel>>(jsonString);
                if (!hotels.Any(x => x.OptionCode == request.OptionCode))
                {
                    return new BookRes
                    {
                        Message = Book.BookingCodeDoesNotExist
                    };
                }
            }

            var booking = new Booking()
            {
                BookingCode = request.OptionCode,
                SleepingTime = GetSleepingTime(30, 60),
                SearchReq = request.SearchReq
            };
            cacheRepository.Set(Book.Booking, booking.BookingCode, booking);

            return new BookRes
            {
                BookingCode = booking.BookingCode,
                BookingTime = booking.BookingTime
            };
        }

        private int GetSleepingTime(int small, int big)
        {
            Random rng = new Random();
            return rng.Next(small, big);
        }
    }
}
