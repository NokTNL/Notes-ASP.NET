using _10_Entity_Framework.Entities;

namespace _10_Entity_Framework.Services;

public interface ICityInfoRepository
{
    public Task<List<City>> GetCitiesAsync();

    public Task<City?> GetCityByIdAsync(int cityId);
}

                                                      
