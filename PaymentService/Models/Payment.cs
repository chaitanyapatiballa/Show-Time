namespace PaymentService.Models
{
    public class Payment
    {
        public int Id { get; set; }
        public string UserId { get; set; } = null!;
        public int BookingId { get; set; }
        public decimal Amount { get; set; }
        public DateTime PaymentTime { get; set; }
        public bool IsSuccessful { get; set; }
    }
}
