using System.ComponentModel.DataAnnotations;

namespace _5_Controllers.Models;

public class UpdateCityDto
{
    [Required(ErrorMessage = "Name cannot be empty!")]
    [MaxLength(50)]
    public string Name { get; set; } = "";

    [MaxLength(200)]
    public string? Description { get; set; }
}