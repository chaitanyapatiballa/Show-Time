using System;
using System.Collections.Generic;

namespace DBModels.Models;

public partial class Booking
{
    public int Bookingid { get; set; }

    public int Userid { get; set; }

    public string? Seatnumber { get; set; }

    public string? Status { get; set; }

    public DateTime Showtime { get; set; }

    public DateTime Bookingtime { get; set; }

    public int? Movieid { get; set; }

    public int? Theaterid { get; set; }

    public virtual ICollection<Billingsummary> Billingsummaries { get; set; } = new List<Billingsummary>();

    public virtual Movie? Movie { get; set; }

    public virtual Payment? Payment { get; set; }

    public virtual Theater? Theater { get; set; }
    public int Showinstanceid { get; set; }
    public int Seatid { get; set; }
    public DateTime BookedAt { get; set; }
}
