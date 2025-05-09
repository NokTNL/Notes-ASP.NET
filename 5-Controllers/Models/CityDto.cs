using System.ComponentModel.DataAnnotations;

namespace _5_Controllers.Models;

public class CityDto
{
    [Required]
    public required int Id { get; set; }

    [Required]
    public required string Name { get; set; }
    public string? Description { get; set; }
}
