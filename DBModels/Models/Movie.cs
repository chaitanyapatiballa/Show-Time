﻿using System;
using System.Collections.Generic;

namespace DBModels.Models;

public partial class Movie
{
    public int Movieid { get; set; }

    public string Title { get; set; } = null!;

    public string? Duration { get; set; }

    public DateOnly? Releasedate { get; set; }

    public string? Genre { get; set; }

    //public virtual ICollection<Booking> Bookings { get; set; } = new List<Booking>();

    //public virtual ICollection<Showtemplate> Showtemplates { get; set; } = new List<Showtemplate>();

    //public virtual ICollection<Theater> Theaters { get; set; } = new List<Theater>();
}
