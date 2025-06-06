﻿using System;
using System.Collections.Generic;

namespace Booking_Service.Models;

public partial class Theater
{
    public int Id { get; set; }

    public string? Name { get; set; }

    public string? Location { get; set; }

    public int Capacity { get; set; }

    public virtual ICollection<Booking> Bookings { get; set; } = new List<Booking>();

    public virtual ICollection<Movie> Movies { get; set; } = new List<Movie>();
}
