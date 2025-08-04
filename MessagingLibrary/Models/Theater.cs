using System;
using System.Collections.Generic;

namespace MessagingLibrary.Models;

public partial class Theater
{
    public int Theaterid { get; set; }

    public string? Name { get; set; }

    public string? Location { get; set; }

    public int? Capacity { get; set; }

    public virtual ICollection<Booking> Bookings { get; set; } = new List<Booking>();

    public virtual ICollection<Seat> Seats { get; set; } = new List<Seat>();

    public virtual ICollection<Showtemplate> Showtemplates { get; set; } = new List<Showtemplate>();

    public virtual ICollection<Movie> Movies { get; set; } = new List<Movie>();
}
