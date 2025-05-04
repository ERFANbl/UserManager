using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Usermanager.Interfaces;
using Usermanager.Model.DTO;
using Usermanager.Model.Entity;

namespace Usermanager.Controllers;

[ApiController]
[Route("api/accessibilities")]
public class AccessibilitiesController : ControllerBase
{
    private readonly IAccessibilityService _accessibilityService;

    public AccessibilitiesController(IAccessibilityService accessibilityService)
    {
        _accessibilityService = accessibilityService;
    }

    [HttpGet]
    public List<string> GetAllAccessibilities()
    {
        var accessibilities = _accessibilityService.GetAllAccessibilitiesAsync();
        return accessibilities;
    }
}

