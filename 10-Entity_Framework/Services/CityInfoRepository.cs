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
        return await _context.Cities.FirstOrDefaultAsync(city => city.Id == cityId);
    }
}

                                                      
