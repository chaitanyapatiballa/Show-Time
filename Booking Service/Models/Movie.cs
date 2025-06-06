using System;
using System.Collections.Generic;

namespace Booking_Service.Models;

public partial class Movie
{
    public int Id { get; set; }

    public string? Title { get; set; }

    public string? Genre { get; set; }

    public int Duration { get; set; }

    public virtual ICollection<Booking> Bookings { get; set; } = new List<Booking>();

    public virtual ICollection<Theater> Theaters { get; set; } = new List<Theater>();
}
