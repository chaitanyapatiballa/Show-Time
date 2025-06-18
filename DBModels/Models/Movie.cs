using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace DBModels.Models;

public partial class Movie
{
    public int Movieid { get; set; }

    public string Title { get; set; } = null!;

    public string? Duration { get; set; }

    public DateTime? releasedate { get; set; }

    public string? Genre { get; set; }

    public virtual ICollection<Booking> Bookings { get; set; } = new List<Booking>();

    public virtual ICollection<Showtemplate> Showtemplates { get; set; } = new List<Showtemplate>();

    [JsonIgnore]
    public ICollection<Movietheater> MovieTheaters { get; set; } = new List<Movietheater>();

}
