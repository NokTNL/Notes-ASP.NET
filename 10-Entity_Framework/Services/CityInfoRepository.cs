using System.Threading.Tasks;
using _10_Entity_Framework.DbContexts;
using _10_Entity_Framework.Entities;
using Microsoft.EntityFrameworkCore;

namespace _10_Entity_Framework.Services;

// So far we are doing operations to our "DB" directly in our controller, which could have some duplications between controllers.
// A REPOSITORY centralises all operations we do to read/write to our DB so to make these operations reusable
// It also abstracts out the actual DB opertaions, allowing dependency injection in controller tests

// In summary, the stream of operations:
//
// [ Incoming request ] <- DTO -> [ Controller ] <- Repository -> [ DB (defined by DbContext & Entities) ]
//     
public class CityInfoRepository(CityInfoContext context): ICityInfoRepository
{
    // Inject DBContext for EF Core
    private readonly CityInfoContext _context = context;
    public async Task<List<City>> GetCitiesAsync()
    {
        // Once defined in DbContext, getting each DbSet represents the actual operation of querying the table in the DB
        // Make sure to use async methods here
        return await _context.Cities.ToListAsync();
    }

    public async Task<City?> GetCityByIdAsync(int cityId)
    {
        return await _context.Cities
        // We want to query for RELATED ENTITIES in another table. To do this we need to use `.Include` method in EF Core
        // Under the hood it compiles into a `JOIN` SQL query
        .Include(city => city.PointsOfInterests)
        .FirstOrDefaultAsync(city => city.Id == cityId);
    }

    public async Task AddCityAsync(City city)
    {
        // Need to retreive the DbSet
        var cities = _context.Cities;
        // This will add entity to our query result but it won't affect the DB yet. See `SaveChangesAsync` below
        await cities.AddAsync(city);
    }

    // EF Core TRACKS changes you have made to entites (add, update, delete)
    public async Task<bool> SaveChangesAsync()
    {
        // Returns true if at least 1 entity has been changed
        return await _context.SaveChangesAsync() >= 0;
    }

}

                                                      
