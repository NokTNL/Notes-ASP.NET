using _10_Entity_Framework.Models;

namespace _10_Entity_Framework;

public class CityDataStore
{
    public List<CityDto> Cities {get; set;}

    // Create a singleton for this class, which is instantiated once declared
    public static CityDataStore Current { get; } = new CityDataStore();

    // Create the store in the constructor
    CityDataStore()
    {
        Cities =
        [
            new()
            {
                Id = 1,
                Name = "New York City",
                Description = "That one with a big park"
            },
            new()
            {
                Id = 2,
                Name = "Paris"
            }
        ];
    }
}
