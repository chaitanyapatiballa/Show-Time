

namespace DBModels.Db;

public enum BookingStatus { Pending, Confirmed, Cancelled, Failed }

public class Booking
{
    public int Id { get; set; }
    public int UserId { get; set; }
    public int MovieId { get; set; }
    public Movie Movie { get; set; } = null!;
    public int TheaterId { get; set; }
    public Theater Theater { get; set; } = null!;
    public int? SeatNumber { get; set; }
    public DateTime BookingTime { get; set; } = DateTime.UtcNow;
    public bool IsCancelled { get; set; } = false;
    public BookingStatus Status { get; set; } = BookingStatus.Pending;
    public Payment? Payment { get; set; }
}