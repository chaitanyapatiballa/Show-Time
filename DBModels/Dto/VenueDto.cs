namespace DBModels.Dto;

public class VenueDto
{
    public string Name { get; set; } = string.Empty;
    public string Location { get; set; } = string.Empty;
    public string Address { get; set; } = string.Empty;
    public int TotalCapacity { get; set; }
}

public class VenueSectionDto
{
    public string Name { get; set; } = string.Empty;
    public int Capacity { get; set; }
    public decimal BasePriceMultiplier { get; set; }
    public int RowCount { get; set; }
    public int SeatsPerRow { get; set; }
}
