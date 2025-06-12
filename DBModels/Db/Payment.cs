
namespace DBModels.Db;

public class Payment
{
    public int PaymentId { get; set; }
    public int BookingId { get; set; }
    public string UserId { get; set; }
    public decimal Amount { get; set; }
    public DateTime PaymentTime { get; set; }
    public bool IsSuccessful { get; set; }
    public Booking Booking { get; set; }

}
