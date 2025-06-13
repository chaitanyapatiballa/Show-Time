using System;
using System.Collections.Generic;

namespace DBModels.Models;

public partial class Showtemplate
{
    public int Showtemplateid { get; set; }

    public int? Movieid { get; set; }

    public int? Theaterid { get; set; }

    public string? Language { get; set; }

    public string? Format { get; set; }

    public decimal? Baseprice { get; set; }

    public virtual Movie? Movie { get; set; }

    public virtual ICollection<Showinstance> Showinstances { get; set; } = new List<Showinstance>();

    public virtual Theater? Theater { get; set; }
}
