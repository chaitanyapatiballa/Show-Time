namespace BookingService.DTOs
{
    public class BookingDto
    {
        public int UserId { get; set; }
        public int MovieId { get; set; }
        public int TheaterId { get; set; }
        public string SeatNumber { get; set; }
        public DateTime ShowTime { get; set; }
        public string? CouponCode { get; set; }
        public string? PaymentMethod { get; set; } 
    }
}
