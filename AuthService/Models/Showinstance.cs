using System;
using System.Collections.Generic;

namespace AuthService.Models;

public partial class Showinstance
{
    public int Showinstanceid { get; set; }

    public int? Showtemplateid { get; set; }

    public DateOnly? Showdate { get; set; }

    public TimeOnly? Showtime { get; set; }

    public int? Availableseats { get; set; }

    public virtual ICollection<Showseatstatus> Showseatstatuses { get; set; } = new List<Showseatstatus>();

    public virtual Showtemplate? Showtemplate { get; set; }
}
