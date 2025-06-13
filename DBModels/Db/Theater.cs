using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace DBModels.Db;

public class Theater
{
    public int TheaterId { get; set; }
    public string Name { get; set; }
    public string Location { get; set; }
    public int Capacity { get; set; }

    public ICollection<Booking> Bookings { get; set; } = new List<Booking>();
    public ICollection<MovieTheater> MovieTheaters { get; set; } = new List<MovieTheater>();
}
