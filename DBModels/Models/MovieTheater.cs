using DBModels.Models;

public class MovieTheater
{
    public int Movieid { get; set; }
    public int Theaterid { get; set; }

    public Movie Movie { get; set; } = null!;
    public Theater Theater { get; set; } = null!;
}