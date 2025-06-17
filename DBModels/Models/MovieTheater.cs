using System.ComponentModel.DataAnnotations.Schema;

namespace DBModels.Models;

public class MovieTheater
{
    public int Movieid { get; set; }
    public Movie Movie { get; set; }

    public int Theaterid { get; set; }
    public Theater Theater { get; set; }
}
