using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DBModels.Models;

public enum EventType
{
    Movie,
    Concert,
    Sport,
    Play,
    StandUpComedy
}

public class Event
{
    [Key]
    public int EventId { get; set; }

    [Required]
    [MaxLength(200)]
    public string Title { get; set; } = string.Empty;

    public string Description { get; set; } = string.Empty;

    public string Language { get; set; } = string.Empty;

    public int DurationMinutes { get; set; }

    public EventType Type { get; set; }

    public string Genre { get; set; } = string.Empty;

    public string ImageUrl { get; set; } = string.Empty;

    public string Cast { get; set; } = string.Empty; // JSON or comma-separated

    public string Director { get; set; } = string.Empty;

    public DateTime ReleaseDate { get; set; }

    // Navigation properties
    public ICollection<Showtemplate> Showtemplates { get; set; } = new List<Showtemplate>();
}
