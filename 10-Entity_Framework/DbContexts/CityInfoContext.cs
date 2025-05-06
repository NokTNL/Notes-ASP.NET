using _10_Entity_Framework.Entities;
using Microsoft.EntityFrameworkCore;

namespace _10_Entity_Framework.DbContexts;

// We need a DbContext to specify the list of entities in the Db
// - an app can have multiple DbContexts
// Our contructor needs to call the base constructor, passing in the options we receive
public class CityInfoContext(DbContextOptions<CityInfoContext> options): DbContext(options)
{
    public required DbSet<City> Cities { get; set; }
    public required DbSet<PointOfInterest> PointOfInterests { get; set; }

    // Seeding data by overriding the base method on model creation
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<City>().HasData(
            new()
            {
                Id = 1,
                Name = "New York City",
                Description = "That one with a big park",
            },
            new()
            {
                Id = 2,
                Name = "Paris"
            }
        );
        modelBuilder.Entity<PointOfInterest>().HasData(
            new()
            {
                Id = 3,
                Name = "Central Park",
                Description = "The big park",
                CityId = 1
            },
            new()
            {
                Id = 4,
                Name = "Eiffel Tower",
                CityId = 2
            }
        );
        base.OnModelCreating(modelBuilder);
    }
}
