using System;
using System.Collections.Generic;

namespace Booking_Service.Models;

public partial class Booking
{
    public int Id { get; set; }

    public int UserId { get; set; }

    public int MovieId { get; set; }

    public int TheaterId { get; set; }

    public int? SeatNumber { get; set; }

    public DateTime BookingTime { get; set; }

    public bool IsCancelled { get; set; }

    public string Status { get; set; } = null!;

    public virtual Movie Movie { get; set; } = null!;

    public virtual Payment? Payment { get; set; }

    public virtual Theater Theater { get; set; } = null!;
}
