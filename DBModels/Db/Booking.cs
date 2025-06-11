namespace DBModels.Db;

public partial class Booking
{
    public int Id { get; set; }
    public string UserId { get; set; } = null!;
    public int MovieId { get; set; }
    public int TheaterId { get; set; }
    public int PaymentId { get; set; }
    public string SeatNumber { get; set; } = null!;
    public DateTime BookingTime { get; set; }

    public virtual Movie Movie { get; set; } = null!;
    public virtual Theater Theater { get; set; } = null!;
    public virtual Payment Payment { get; set; } = null!;
}
