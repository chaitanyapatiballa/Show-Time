namespace Booking_Service.Models
{
    public class Booking
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public int TheaterId { get; set; }
        public int MovieId { get; set; }
        public int? SeatNumber { get; set; }
        public DateTime BookingTime { get; set; }
        public bool IsCancelled { get; set; } = false;


    }
}
