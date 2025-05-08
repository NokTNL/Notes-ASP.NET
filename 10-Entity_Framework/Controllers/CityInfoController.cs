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
    public async Task<ActionResult<List<CityWithoutPointsOfInterestDto>>> GetCities()
    {
        var cityEntities = await _cityInfoRepository.GetCitiesAsync();
        // Remember entities are NOT the same as the DTOs, so we may need to map it
        // Here the major difference is that we want to omit the `pointOfInterest` array
        var citiesDtos = new List<CityWithoutPointsOfInterestDto>();
        foreach(var city in cityEntities)
        {
            citiesDtos.Add(new(){
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
        var cityDto = new CityDto()
        {
            Id = cityEntity.Id,
            Name = cityEntity.Name,
            Description = cityEntity.Description,
            PointsOfInterest = []
        };
        foreach(var pointOfInterest in cityEntity.PointsOfInterests)
        {
            cityDto.PointsOfInterest.Add(new PointOfInterestDto() {
                Id = pointOfInterest.Id,
                Name = pointOfInterest.Name,
                Description = pointOfInterest.Description,
                CityId = pointOfInterest.CityId
            });
        }
        return Ok(cityDto);
    }

    [HttpPost(Name = "CreateCity")]
    public async Task<ActionResult<CityDto>> CreateCity(NewCityDto newCity)
    {
        // The City and the associated PointOfInterest's do NOT have a proper Id yet!
        var newCityEntity = new City(){
            Name = newCity.Name,
            Description = newCity.Description,
            PointsOfInterests = newCity.PointsOfInterest.Select(p => new PointOfInterest(){
                Name = p.Name,
                Description = p.Description
            }).ToList()
        };
        await _cityInfoRepository.AddCityAsync(newCityEntity);
        // After this they are inserted into the DB and SHOULD have an Id
        await _cityInfoRepository.SaveChangesAsync();
        return CreatedAtRoute("GetCityById", new { cityId = newCityEntity.Id }, null);
    }

    [HttpPut("{cityId}", Name = "UpdateCity")]
    public async Task<ActionResult> UpdateCity(int cityId, UpdateCityDto updatedCity)
    {
        var city = await _cityInfoRepository.GetCityByIdAsync(cityId);
        if (city == null)
        {
            return NotFound();
        }
        city.Name = updatedCity.Name;
        city.Description = updatedCity.Description;
        city.PointsOfInterests = updatedCity.PointsOfInterest.Select(p => new PointOfInterest(){
                Name = p.Name,
                Description = p.Description
            }).ToList();
        await _cityInfoRepository.SaveChangesAsync();
        return NoContent();
    }

    [HttpPatch("{cityId}", Name="PartiallyUpdateCity")]
    public async Task<ActionResult> PartiallyUpdateCity(
        int cityId,
        JsonPatchDocument<UpdateCityDto> patchDocument
    )
    {
        var city = await _cityInfoRepository.GetCityByIdAsync(cityId);
        if (city == null)
        {
            return NotFound();
        }
        // Entity => Dto for applying JsonPatchDocument
        var updatedCity = new UpdateCityDto
        {
            Name = city.Name,
            Description = city.Description,
            PointsOfInterest = city.PointsOfInterests.Select(p => new PointOfInterestDto()
            {
                Id = p.Id,
                Name = p.Name,
                Description = p.Description,
                CityId = p.CityId
            }).ToList()
        };
        // Applying JsonPatchDocument to the DTO
        patchDocument.ApplyTo(updatedCity, ModelState);
        if (!ModelState.IsValid
          || !TryValidateModel(updatedCity)
        )
        {
            return BadRequest(ModelState);
        }
        // Dto => Entity
        city.Name = updatedCity.Name;
        city.Description = updatedCity.Description;
        city.PointsOfInterests = updatedCity.PointsOfInterest.Select(p => new PointOfInterest()
        {
            Id = p.Id,
            Name = p.Name,
            Description = p.Description,
            CityId = p.CityId
        }).ToList();
        await _cityInfoRepository.SaveChangesAsync();
        return NoContent();
    }
}
