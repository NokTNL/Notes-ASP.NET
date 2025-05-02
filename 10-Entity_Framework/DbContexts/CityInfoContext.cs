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
}
