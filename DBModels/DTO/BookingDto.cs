namespace Booking_Service.DTOs
{
    public class BookingDto
    {
        public int UserId { get; set; }
        public int MovieId { get; set; }
        public int TheaterId { get; set; }
        public string SeatNumber { get; set; } = string.Empty;
    }
}
