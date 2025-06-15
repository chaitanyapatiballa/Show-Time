namespace BookingService.DTOs;

public class BookingDto
{
    public int Movieid { get; set; }
    public int Theaterid { get; set; }
    public int Userid { get; set; }
    public string Seatnumber { get; set; } = string.Empty;
    public DateTime Showtime { get; set; }
}

