using System;
using System.Collections.Generic;

namespace MessagingLibrary.Models;

public partial class Booking
{
    public int Bookingid { get; set; }

    public int Userid { get; set; }

    public int? Movieid { get; set; }

    public int? Theaterid { get; set; }

    public int? Seatid { get; set; }

    public string? Seatnumber { get; set; }

    public int? Showinstanceid { get; set; }

    public DateTime? Showtime { get; set; }

    public DateTime? Bookingtime { get; set; }

    public DateTime? Cancelledat { get; set; }

    public string? Status { get; set; }

    public decimal? Baseprice { get; set; }

    public decimal? Refundamount { get; set; }

    public bool? Emailsent { get; set; }

    public bool? Isrefunded { get; set; }

    public virtual ICollection<Billingsummary> Billingsummaries { get; set; } = new List<Billingsummary>();

    public virtual Movie? Movie { get; set; }

    public virtual ICollection<Payment> Payments { get; set; } = new List<Payment>();

    public virtual Seat? Seat { get; set; }

    public virtual Showinstance? Showinstance { get; set; }

    public virtual Theater? Theater { get; set; }

    public virtual User User { get; set; } = null!;
}
