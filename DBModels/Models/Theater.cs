using System;
using System.Collections.Generic;

namespace DBModels.Models;

public partial class Theater
{
    public int Theaterid { get; set; }

    public string Name { get; set; } = null!;

    public string Location { get; set; } = null!;

    public virtual ICollection<Booking> Bookings { get; set; } = new List<Booking>();

    public virtual ICollection<Showtemplate> Showtemplates { get; set; } = new List<Showtemplate>();

    public virtual ICollection<Movie> Movies { get; set; } = new List<Movie>();
}
