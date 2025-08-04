using System;
using System.Collections.Generic;

namespace MessagingLibrary.Models;

public partial class User
{
    public int Userid { get; set; }

    public string? Username { get; set; }

    public string? Email { get; set; }

    public byte[]? Passwordhash { get; set; }

    public byte[]? Passwordsalt { get; set; }

    public string? Role { get; set; }

    public virtual ICollection<Booking> Bookings { get; set; } = new List<Booking>();

    public virtual ICollection<Payment> Payments { get; set; } = new List<Payment>();
}
