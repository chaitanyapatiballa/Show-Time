using System;
using System.Collections.Generic;

namespace DBModels.Models;

public partial class Seat
{
    public int Seatid { get; set; }

    public int Theaterid { get; set; }

    public string Row { get; set; } = null!;

    public int Number { get; set; }

    public decimal Baseprice { get; set; }

    public virtual ICollection<Booking> Bookings { get; set; } = new List<Booking>();

    public virtual ICollection<Showseatstatus> Showseatstatuses { get; set; } = new List<Showseatstatus>();

    public virtual Theater Theater { get; set; } = null!;
}
