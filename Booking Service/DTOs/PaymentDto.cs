namespace Booking_Service.DTOs
{
    public class PaymentDto
    {
        public string UserId { get; set; } = null!;
        public int BookingId { get; set; }
        public decimal Amount { get; set; }
        public DateTime PaymentTime { get; set; }
        public bool IsSuccessful { get; set; }
    }
}
