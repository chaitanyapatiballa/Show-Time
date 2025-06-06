
namespace DBModels.Db;

public class MovieTheater
{
    public int MovieId { get; set; }
    public Movie Movie { get; set; } = null!;
    public int TheaterId { get; set; }
    public Theater Theater { get; set; } = null!;
}