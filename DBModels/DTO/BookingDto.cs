namespace BookingService.DTOs
{
    public class BookingDto
    {
        public string UserId { get; set; }
        public int MovieId { get; set; }
        public int TheaterId { get; set; }
        public int PaymentId { get; set; }
        public string SeatNumber { get; set; }
        public DateTime BookingTime { get; set; }
    }
}
