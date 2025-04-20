using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using Usermanager.Interfaces;

[ApiController]
[Route("api/[controller]")]
[EnableCors("AllowMyFrontend")] // Use the policy name you define
public class LocationController : ControllerBase
{
    private readonly ILocationService _locationService;

    public LocationController(ILocationService locationService)
    {
        _locationService = locationService;
    }

    [HttpGet]
    [Route("provinces")]
    public IActionResult GetProvinces()
    {
        return Ok(_locationService.GetProvinces());
    }

    [HttpGet]
    [Route("cities")]
    public IActionResult GetCities([FromQuery] int provinceId)
    {
        var cities = _locationService.GetCitiesByProvinceId(provinceId);
        return Ok(cities);
    }
}
