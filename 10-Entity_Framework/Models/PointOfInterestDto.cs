using System.ComponentModel.DataAnnotations;

public class PointOfInterestDto
{
    [Required]
    public required int Id { get; set; }

    [Required]
    public required string Name { get; set; }

    public string? Description { get; set; }

    [Required]
    public required int CityId { get; set; }
}