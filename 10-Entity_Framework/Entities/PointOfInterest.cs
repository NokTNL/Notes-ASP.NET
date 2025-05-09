using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace _10_Entity_Framework.Entities;

public class PointOfInterest
{
    [Required]
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int Id { get; set; }

    [Required]
    [MaxLength(50)]
    public required string Name { get; set; }

    [MaxLength(200)]
    public string? Description { get; set; }

    // By default, if a class contains a navigation to another entity, this will look for another property in the entity that has `*-Id` attached to the navigation's name
    // We can also explicitly define this with `ForeignKeyAttribute`, specifying what is used as the key to point to another entity
    /* [ForeignKey("CityId")] */
    public City? City { get; set; }
    // Here `CityId` will be used as the default foreign key
    
    [Required]
    public int CityId { get; set; }

}
