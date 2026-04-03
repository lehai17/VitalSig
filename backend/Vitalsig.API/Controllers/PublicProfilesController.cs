using Microsoft.AspNetCore.Mvc;
using Vitalsig.API.Application.PublicProfiles;
using Vitalsig.API.Application.PublicProfiles.Contracts;

namespace Vitalsig.API.Controllers;

[ApiController]
[Route("api/public/p")]
public class PublicProfilesController(IPublicProfileService publicProfileService) : ControllerBase
{
    [HttpGet("{token}")]
    public async Task<IActionResult> GetByToken(string token, CancellationToken cancellationToken)
    {
        var profile = await publicProfileService.GetPublicProfileByTokenAsync(token, cancellationToken);
        return profile is null ? NotFound() : Ok(profile);
    }

    [HttpPost("{token}/scan")]
    public async Task<IActionResult> CreateScanLog(
        string token,
        [FromBody] CreateScanLogRequest request,
        CancellationToken cancellationToken)
    {
        var ipAddress = HttpContext.Connection.RemoteIpAddress?.ToString();
        var userAgent = Request.Headers.UserAgent.ToString();

        var scanLog = await publicProfileService.CreateScanLogAsync(
            token,
            request,
            ipAddress,
            userAgent,
            cancellationToken);

        return scanLog is null ? NotFound() : Ok(scanLog);
    }
}
