using Microsoft.AspNetCore.Mvc;

namespace _5_Middlewares.Controllers;

public class City()
{
    public required int Id {get; set;}
    public required string  Name {get; set;}
}

// To create a controller, create a new class that extends the ControllerBase class from MVC
// Also add `[ApiController]` attribute to enable useful features like request data validation
[ApiController]
// You usually want each controller to correspond to one route, here it will be `cities`
[Route("cities")]
public class CityInfoController: ControllerBase
{
    // Each method usually corresponds to one HTTP verb,
    [HttpGet]
    // It returns a JsonResult (a class defined by MVC)
    public JsonResult GetCities()
    {
        return new JsonResult(new List<City>
        {
            new() {Id = 1, Name = "New York City"},
            new() {Id = 2, Name = "Paris"}
        });
    }
}
