using DBModels.Models;

public class MovieTheater
{
    public int? movieid { get; set; }   
    public int? theaterid { get; set; }
        
    public Movie Movie { get; set; } = null!;
    public Theater Theater { get; set; } = null!;
}