using System;
using System.Collections.Generic;

namespace DBModels.Models;

public partial class Payment
{
    public int Paymentid { get; set; }

    public int Userid { get; set; }

    public decimal Amountpaid { get; set; }

    public DateTime Paymentdate { get; set; }

    public string Paymentmethod { get; set; } = null!;

    public int? Bookingid { get; set; }

    public string? Status { get; set; }

    public DateTime? Refunddate { get; set; }

    public virtual Booking? Booking { get; set; }

    public virtual User User { get; set; } = null!;
}
