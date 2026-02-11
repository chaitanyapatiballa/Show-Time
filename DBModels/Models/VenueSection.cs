using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DBModels.Models;

public class VenueSection
{
    [Key]
    public int SectionId { get; set; }

    public int VenueId { get; set; }

    [Required]
    [MaxLength(50)]
    public string Name { get; set; } = string.Empty; // e.g., "VIP", "Stand A", "Balcony"

    public int Capacity { get; set; }

    public decimal BasePriceMultiplier { get; set; } = 1.0m;

    public int RowCount { get; set; }
    public int SeatsPerRow { get; set; }

    [ForeignKey("VenueId")]
    public Venue? Venue { get; set; }
}
