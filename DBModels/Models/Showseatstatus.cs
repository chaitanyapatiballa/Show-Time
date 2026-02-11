using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DBModels.Models;

public partial class Showseatstatus
{
    public int Showseatstatusid { get; set; }

    public int Showinstanceid { get; set; }

    public int Seatid { get; set; }

    public bool Isbooked { get; set; }

    public DateTime? LockedAt { get; set; }

    public string? LockedBy { get; set; } // ConnectionId or UserId

    public virtual Seat Seat { get; set; } = null!;

    public virtual Showinstance Showinstance { get; set; } = null!;

    [Timestamp]
    public byte[] RowVersion { get; set; } = null!;
}
