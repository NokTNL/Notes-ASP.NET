using System.Threading.Tasks;
using _10_Entity_Framework.Entities;
using _10_Entity_Framework.Models;
using _10_Entity_Framework.Services;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;

namespace _10_Entity_Framework.Controllers;

[ApiController]
[Route("cities")]
// Once declared as an dependency in this constructor, ASP.NET will find the relavant service in the builder and inject into this controller
public class CityInfoController(ICityInfoRepository cityInfoRepository): ControllerBase
{
    // Store the dependency here as a private field
    public readonly ICityInfoRepository _cityInfoRepository = cityInfoRepository;

    [HttpGet(Name = "GetCities")]
    public async Task<ActionResult<List<CityDto>>> GetCities()
    {
        var cityEntities = await _cityInfoRepository.GetCitiesAsync();
        // Remember entities are NOT the same as the DTOs, so we may need to map it
        // Here the major difference is that we want to omit the `pointOfInterest` array
        var citiesDtos = new List<CityDto>();
        foreach(var city in cityEntities)
        {
            citiesDtos.Add( new(){
                Id = city.Id,
                Name = city.Name,
                Description = city.Description
            });
        }
        return Ok(citiesDtos);
    }

    [HttpGet("{cityId}", Name = "GetCityById")]
    public async Task<ActionResult<CityDto>> GetCityById(int cityId)
    {
        var cityEntity = await _cityInfoRepository.GetCityByIdAsync(cityId);
        if (cityEntity == null)
        {
            return NotFound();
        }
        return Ok(new CityDto(){
            Id = cityEntity.Id,
            Name = cityEntity.Name,
            Description = cityEntity.Description
        });
    }

    // [HttpPost(Name = "CreateCity")]
    // public ActionResult<CityDto> CreateCity(/*[FromBody]*/ NewCityDto newCity)
    // {
    //     var maxCityId = CityDataStore.Current.Cities.Select(city => city.Id).Max();
    //     var newCityWithId = new CityDto()
    //     {
    //         Id = maxCityId + 1,
    //         Name = newCity.Name,
    //         Description = newCity.Description
    //     };
    //     CityDataStore.Current.Cities.Add(newCityWithId);
    //     return CreatedAtRoute("GetCityById", new { cityId = newCityWithId.Id }, newCityWithId);
    // }

    // [HttpPut("{cityId}", Name = "UpdateCity")]
    // public ActionResult UpdateCity(int cityId, UpdateCityDto updatedCity)
    // {
    //     var city = CityDataStore.Current.Cities.FirstOrDefault(city => city.Id == cityId);
    //     if (city == null)
    //     {
    //         return NotFound();
    //     }
    //     city.Name = updatedCity.Name;
    //     city.Description = updatedCity.Description;
    //     return NoContent();
    // }

    // [HttpPatch("{cityId}", Name="PartiallyUpdateCity")]
    // public ActionResult PartiallyUpdateCity(
    //     int cityId,
    //     JsonPatchDocument<UpdateCityDto> patchDocument /* With NewtonsoftJson, request body will be parsed into a JsonPatchDocument */
    // )
    // {
    //     var city = CityDataStore.Current.Cities.FirstOrDefault(city => city.Id == cityId);
    //     if (city == null)
    //     {
    //         return NotFound();
    //     }
    //     var updatedCity = new UpdateCityDto
    //     {
    //         Name = city.Name,
    //         Description = city.Description
    //     };
    //     patchDocument.ApplyTo(updatedCity, ModelState); // `ApplyTo` will apply the new patches.
    //     if (!ModelState.IsValid
    //       || !TryValidateModel(updatedCity)
    //     )
    //     {
    //         return BadRequest(ModelState);
    //     }
    //     city.Name = updatedCity.Name;
    //     city.Description = updatedCity.Description;
    //     return NoContent();
    // }
}
