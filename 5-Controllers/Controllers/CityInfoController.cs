using _5_Controllers.Models;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;

namespace _5_Controllers.Controllers;

// To create a controller, create a new class that extends the ControllerBase class from MVC
// Also add `[ApiController]` attribute to enable useful features like request data validation
[ApiController]
// You usually want each controller to correspond to one route, here it will be `cities`
[Route("cities")]
public class CityInfoController: ControllerBase
{
    // Each method usually corresponds to one HTTP verb
    // You can assign the route Name property to each method for referring back to later on
    [HttpGet(Name = "GetCities")]
    // Returns a ActionResult, which will be converted to JsonResult under the hood with the proper status code attached
    // !!! Specify your return type here because it will show up in the schema! (even though it still compiles when there's a mismatch)
    public ActionResult<List<CityDto>> GetCities()
    {
        // `Ok` is a helper method from ControllerBase that corresponds to 200
        return Ok(CityDataStore.Current.Cities);
    }

    // Routes can be nested, and can also be defined inside `[Http*]` so you don't need a separate [Route()]
    // If you define a route param, you will receive that as the method's argument, and NAME HAS TO MATCH
    // - You can add `[FromRoute]` to the parameter below, but that's the default behaviour for route params defined in the attribute so not needed
    [HttpGet("{cityId}", Name = "GetCityById")]
    public ActionResult<CityDto> GetCityById(/*[FromRoute]*/ int cityId)
    {
        var city = CityDataStore.Current.Cities.FirstOrDefault(city => city.Id == cityId);
        if (city == null)
        {
            return NotFound();
        }
        return Ok(city);
    }

    // If you declare complex types in your method param, it will be inferred as `[FromBody]` i.e. parsed from the request body
    // e.g. useful for a POST request
    [HttpPost(Name = "CreateCity")]
    public ActionResult<CityDto> CreateCity(/*[FromBody]*/ NewCityDto newCity)
    {
        var maxCityId = CityDataStore.Current.Cities.Select(city => city.Id).Max();
        var newCityWithId = new CityDto()
        {
            Id = maxCityId + 1,
            Name = newCity.Name,
            Description = newCity.Description
        };
        CityDataStore.Current.Cities.Add(newCityWithId);
        // Return a 201, specifying:
        // 1. The route Name you can find the resource loater on; this will be shown in the Location header of the response
        // 2. The route params for generating the route for the newly creating entity, as an object
        // 3. The newly created entity as an object
        return CreatedAtRoute("GetCityById", new { cityId = newCityWithId.Id }, newCityWithId);
    }

    [HttpPut("{cityId}", Name = "UpdateCity")]
    public ActionResult UpdateCity(int cityId, UpdateCityDto updatedCity)
    {
        var city = CityDataStore.Current.Cities.FirstOrDefault(city => city.Id == cityId);
        if (city == null)
        {
            return NotFound();
        }
        city.Name = updatedCity.Name;
        city.Description = updatedCity.Description;
        return NoContent();
    }

    // For partial updates, we should adhere to the JSON Patch document format, which is essentially a list of operations
    // We will need the Microsoft.AspNetCore.JsonPatch NuGet package: https://www.nuget.org/packages/microsoft.aspnetcore.jsonpatch/
    // Also need its dependency Microsoft.AspNetCore.Mvc.NewtonsoftJson: https://www.nuget.org/packages/microsoft.aspnetcore.mvc.newtonsoftjson
    [HttpPatch("{cityId}", Name="PartiallyUpdateCity")]
    public ActionResult PartiallyUpdateCity(
        int cityId,
        JsonPatchDocument<UpdateCityDto> patchDocument /* With NewtonsoftJson, request body will be parsed into a JsonPatchDocument */
    )
    {
        var city = CityDataStore.Current.Cities.FirstOrDefault(city => city.Id == cityId);
        if (city == null)
        {
            return NotFound();
        }
        var updatedCity = new UpdateCityDto
        {
            Name = city.Name,
            Description = city.Description
        };
        patchDocument.ApplyTo(updatedCity, ModelState); // `ApplyTo` will apply the new patches.
        // We optionally pass in the `ModelState`, which logs errors regarding whether incoming request's structure is valid
        // If we do not pass `ModelState` to ApplyTo, when the JsonPatch document is invalid (e.g. wrong `path`), it simply throws and return a 500 back to the user
        if (!ModelState.IsValid
          // We also need to make sure that the object AFTER applying the JsonPatch is actually valid (e.g. not having a Required property removed)
          || !TryValidateModel(updatedCity)
        )
        {
            return BadRequest(ModelState);
        }
        city.Name = updatedCity.Name;
        city.Description = updatedCity.Description;
        return NoContent();
    }
}
