namespace PaymentService.DTOs
{
    public class PaymentDto
    {
        public int UserId;
        public int PaymentId { get; set; }
        public int BookingId { get; set; }
        public decimal AmountPaid { get; set; }
        public string PaymentMethod { get; set; }
        public DateTime PaymentDate { get; set; }
    }
}