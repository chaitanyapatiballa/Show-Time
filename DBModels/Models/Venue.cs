using System.ComponentModel.DataAnnotations;

namespace DBModels.Models;

public class Venue
{
    [Key]
    public int VenueId { get; set; }

    [Required]
    [MaxLength(100)]
    public string Name { get; set; } = string.Empty;

    [Required]
    public string Location { get; set; } = string.Empty;

    public string Address { get; set; } = string.Empty;

    public int TotalCapacity { get; set; }

    // Navigation properties
    public ICollection<VenueSection> Sections { get; set; } = new List<VenueSection>();
}
