using DBModels.Models;

namespace DBModels.Dto;

public class TheaterDto
{
    public int Theaterid { get; set; }
    public int Capacity { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Location { get; set; } = string.Empty;
    public List<int>? MovieIds { get; set; }
    public ICollection<Movietheater> MovieTheaters { get; set; } = new List<Movietheater>();

}
