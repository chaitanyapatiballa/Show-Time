using System;
using System.Collections.Generic;

namespace AuthService.Models;

public partial class Showseatstatus
{
    public int Showseatstatusid { get; set; }

    public int Showinstanceid { get; set; }

    public int Seatid { get; set; }

    public bool Isbooked { get; set; }

    public virtual Seat Seat { get; set; } = null!;

    public virtual Showinstance Showinstance { get; set; } = null!;
}
