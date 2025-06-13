
namespace DBModels.Db;

public class Payment
{
    public int PaymentId { get; set; }
    public int BookingId { get; set; }
    public int UserId { get; set; }
    public decimal AmountPaid { get; set; }
    public DateTime PaymentTime { get; set; }
    public bool IsSuccessful { get; set; }
    public string PaymentMethod { get; set; }
    public DateTime PaymentDate { get; set; }
    public Booking Booking { get; set; }
}
