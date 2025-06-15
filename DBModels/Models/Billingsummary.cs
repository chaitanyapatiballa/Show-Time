using System;
using System.Collections.Generic;

namespace DBModels.Models;

public partial class Billingsummary
{
    public int Billingsummaryid { get; set; }

    public int? Bookingid { get; set; }

    public decimal? Baseamount { get; set; }

    public decimal? Gst { get; set; }

    public decimal? Servicefee { get; set; }

    public decimal? Discount { get; set; }

    public decimal? Totalamount { get; set; }

    public virtual Booking? Booking { get; set; }
}
