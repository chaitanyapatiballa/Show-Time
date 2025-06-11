namespace PaymentService.DTOs
{
    public class PaymentDto
    {
        public int PaymentId { get; set; }       
        public string UserId { get; set; }
        public int BookingId { get; set; }
        public decimal Amount { get; set; }
        public DateTime PaymentTime { get; set; }
        public bool IsSuccessful { get; set; }
    }
}
