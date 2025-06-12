namespace PaymentService.DTOs
{
    public class PaymentDto
    {
        // Nullable so it won't be required in POST input
        public int? PaymentId { get; set; }

        public int BookingId { get; set; }

        public string UserId { get; set; }

        public decimal Amount { get; set; }

        // Nullable to allow server-side setting
        public DateTime? PaymentTime { get; set; }
    }
}