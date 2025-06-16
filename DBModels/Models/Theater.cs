using System;
using System.Collections.Generic;

namespace DBModels.Models;

public partial class Theater
{
    public int Theaterid { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Location { get; set; } = string.Empty;

    public virtual ICollection<Booking> Bookings { get; set; } = new List<Booking>();

    public virtual ICollection<Showtemplate> Showtemplates { get; set; } = new List<Showtemplate>();
    public virtual ICollection<MovieTheater> MovieTheaters { get; set; } = new List<MovieTheater>();


}