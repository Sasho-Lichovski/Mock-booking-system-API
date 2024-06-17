namespace Services.Models.Book
{
    public class Booking
    {
        public string BookingCode { get; set; }
        public int SleepingTime { get; set; }
        public DateTime BookingTime = DateTime.Now;
    }
}
