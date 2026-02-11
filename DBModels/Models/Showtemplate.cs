using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace DBModels.Models;

public partial class Showtemplate
{
    public int Showtemplateid { get; set; }

    public int? EventId { get; set; }
    public int? VenueId { get; set; }

    [ForeignKey("EventId")]
    public Event? Event { get; set; }

    [ForeignKey("VenueId")]
    public Venue? NewVenue { get; set; }

    public int? Movieid { get; set; }

    public int? Theaterid { get; set; }

    [Column("language")]
    public string? Language { get; set; }

    public string? Format { get; set; }

    public decimal? Baseprice { get; set; }

    public virtual Movie? Movie { get; set; }

    public virtual ICollection<Showinstance> Showinstances { get; set; } = new List<Showinstance>();

    public virtual Theater? Theater { get; set; }
}
