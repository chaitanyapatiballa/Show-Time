namespace PaymentService.DTOs
{
    public class PaymentDto
    {
        public string UserId { get; set; } = string.Empty;
        public int BookingId { get; set; }
        public decimal Amount { get; set; }
        public DateTime PaymentTime { get; set; }
        public bool IsSuccessful { get; set; }
    }
}