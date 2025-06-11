using System.ComponentModel.DataAnnotations;

namespace PaymentService.DTOs
{
    public class PaymentDto
    {
        [Required]
        public string UserId { get; set; } = string.Empty;

        [Required]
        public int BookingId { get; set; }

        [Range(0.01, double.MaxValue, ErrorMessage = "Amount must be greater than 0.")]
        public decimal Amount { get; set; }
    }
}
