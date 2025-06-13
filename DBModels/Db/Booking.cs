namespace DBModels.Db;

public class Booking
{
    public int BookingId { get; set; }
    public int UserId { get; set; }
    public int MovieId { get; set; }
    public int TheaterId { get; set; }
    public int? PaymentId { get; set; }
    public string SeatNumber { get; set; }
    public string Status { get; set; }
    public DateTime ShowTime { get; set; }
    public DateTime BookingTime { get; set; }

    public Movie Movie { get; set; }
    public Theater Theater { get; set; }
    public Payment Payment { get; set; }
    public bool IsCancelled { get; set; }
}
