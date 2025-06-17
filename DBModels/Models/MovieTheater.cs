using System.Text.Json.Serialization;
using DBModels.Models;

public class MovieTheater
{
    public int Movieid { get; set; }
    public int Theaterid { get; set; }

    [JsonIgnore]
    public Movie? Movie { get; set; }

    [JsonIgnore]
    public Theater? Theater { get; set; }
}
