// LocationController.cs
// أضف في Program.cs: builder.Services.AddSingleton<LocationService>();
using Microsoft.AspNetCore.Mvc;

[ApiController]
[Route("api/[controller]")]
public class LocationController : ControllerBase
{
    private readonly LocationService _locationService;

    public LocationController(LocationService locationService)
    {
        _locationService = locationService;
    }

    // GET /api/location/governorates
    [HttpGet("governorates")]
    public IActionResult GetGovernorates() =>
        Ok(_locationService.GetGovernorates()
            .Select(g => new { g.Id, g.NameAr, g.NameEn, g.Code }));

    // GET /api/location/centers/4
    [HttpGet("centers/{governorateId}")]
    public IActionResult GetCenters(int governorateId) =>
        Ok(_locationService.GetCenters(governorateId)
            .Select(c => new { c.Id, c.NameAr, c.NameEn, c.Code }));

    // GET /api/location/cities/7
    [HttpGet("cities/{centerId}")]
    public IActionResult GetCities(int centerId)
    {
        var cities = _locationService.GetCities(centerId);

        if (cities == null || !cities.Any())
            return Ok(new List<object>());

        return Ok(cities.Select(c => new { c.Id, c.NameAr, c.NameEn, c.Code }));
    }
}