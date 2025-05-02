using System.ComponentModel.DataAnnotations;

namespace _10_Entity_Framework.Models;

public class CityDto
{
    [Required]
    public required int Id { get; set; }

    [Required]
    public required string Name { get; set; }

    public string? Description { get; set; }
}
