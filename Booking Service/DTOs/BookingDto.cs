namespace Booking_Service.DTOs
{
    public class BookingDto
    {
        public int UserId { get; set; }
        public int TheaterId { get; set; }
        public int MovieId { get; set; }
        public int? SeatNumber { get; set; }
        public DateTime  BookingTime { get; set; } = DateTime.Now;
      
    }
}
