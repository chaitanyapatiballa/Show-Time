namespace DBModels.Db;

public class Booking
{
    public int Id { get; set; }
    public string UserId { get; set; }         
    public int TheaterId { get; set; }
    public string SeatNumber { get; set; }     
    public DateTime BookingTime { get; set; }
    public bool IsCancelled { get; set; }
    public string Status { get; set; }         
    public int PaymentId { get; set; }
}
