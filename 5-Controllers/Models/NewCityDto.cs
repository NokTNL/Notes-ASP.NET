using System.ComponentModel.DataAnnotations;

namespace _5_Controllers.Models;

public class NewCityDto
{
    // We can add `RequiredAttribute` for validation so that user cannot give empty string here
    // - This will also make it required in the Swagger schema
    // - You can optionally customise the error message with `ErrorMessage`
    [Required(ErrorMessage = "Name cannot be empty!")]
    // There are a lot of validation attributes we can use from this namespace, even including things like phone and credit card number
    [MaxLength(50)]
    public string Name { get; set; } = "";

    [MaxLength(200)]
    public string? Description { get; set; }
}
