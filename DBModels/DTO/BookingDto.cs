namespace BookingService.DTOs
{
    public class BookingDto
    {
        public int BookingId { get; set; }
        public int MovieId { get; set; }
        public int TheaterId { get; set; }
        public string SeatNumber { get; set; } = string.Empty;
        public DateTime BookingTime { get; set; }
        public bool IsCancelled { get; set; }
        public string Status { get; set; } = string.Empty;
        public int PaymentId { get; set; }
        public DateTime ShowTime { get; set; }
        public int UserId { get; set; }
    }
}
