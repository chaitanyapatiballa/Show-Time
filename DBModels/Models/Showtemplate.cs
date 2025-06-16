using System;
using System.Collections.Generic;

namespace DBModels.Models;

public partial class Showtemplate
{
    public int Showtemplateid { get; set; }

    public int Movieid { get; set; }
    public int Theaterid { get; set; }
    public string Language { get; set; } = null!;
    public string Format { get; set; } = null!;
    public decimal Baseprice { get; set; }

    public virtual Movie Movie { get; set; } = null!;
    public virtual Theater Theater { get; set; } = null!;
    public virtual ICollection<Showinstance> Showinstances { get; set; } = new List<Showinstance>();
}
