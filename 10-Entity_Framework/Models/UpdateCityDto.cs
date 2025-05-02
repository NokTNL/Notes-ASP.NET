using System.ComponentModel.DataAnnotations;

namespace _10_Entity_Framework.Models;

public class UpdateCityDto
{
    [Required(ErrorMessage = "Name cannot be empty!")]
    [MaxLength(50)]
    public required string Name { get; set; }

    [MaxLength(200)]
    public string? Description { get; set; }
}