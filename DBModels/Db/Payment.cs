
namespace DBModels.Db;
public class Payment
{
    public int Id { get; set; }
    public string UserId { get; set; } = null!;
    public int BookingId { get; set; }
    public Booking Booking { get; set; } = null!;
    public decimal Amount { get; set; }
    public DateTime PaymentTime { get; set; }
    public bool IsSuccessful { get; set; }
}