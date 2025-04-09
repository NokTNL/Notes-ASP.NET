using _5_Controllers.Models;
using Microsoft.AspNetCore.Mvc;

namespace _5_Controllers.Controllers;

// To create a controller, create a new class that extends the ControllerBase class from MVC
// Also add `[ApiController]` attribute to enable useful features like request data validation
[ApiController]
// You usually want each controller to correspond to one route, here it will be `cities`
[Route("cities")]
public class CityInfoController: ControllerBase
{
    // Each method usually corresponds to one HTTP verb,
    [HttpGet]
    // Returns a ActionResult, which will be converted to JsonResult under the hood with the proper status code attached
    // !!! Specify your return type here because it will show up in the schema! (even though it still compiles when there's a mismatch)
    public ActionResult<List<CityDto>> GetCities()
    {
        // `Ok` is a helper method from ControllerBase that corresponds to 200
        return Ok(CityDataStore.Current.Cities);
    }

    // Routes can be nested
    // If you define a route param, you will receive that as the method's argument
    [HttpGet]
    [Route("{id}")]
    public ActionResult<CityDto> GetCityById(int id)
    {
        var city = CityDataStore.Current.Cities.FirstOrDefault(city => city.Id == id);
        if (city == null)
        {
            return NotFound();
        }
        return Ok(city);
    }
}
