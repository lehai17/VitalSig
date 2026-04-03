using Microsoft.AspNetCore.Mvc;
using Vitalsig.API.Application.Profiles;
using Vitalsig.API.Application.Profiles.Contracts;

namespace Vitalsig.API.Controllers;

[ApiController]
[Route("api/profiles")]
public class ProfilesController(IProfileService profileService) : ControllerBase
{
    [HttpGet]
    public async Task<IActionResult> GetProfiles([FromQuery] Guid? ownerUserId, CancellationToken cancellationToken)
    {
        var profiles = await profileService.GetProfilesAsync(ownerUserId, cancellationToken);
        return Ok(profiles);
    }

    [HttpGet("{id:guid}")]
    public async Task<IActionResult> GetProfileById(Guid id, CancellationToken cancellationToken)
    {
        var profile = await profileService.GetProfileByIdAsync(id, cancellationToken);
        return profile is null ? NotFound() : Ok(profile);
    }

    [HttpPost]
    public async Task<IActionResult> CreateProfile([FromBody] CreateProfileRequest request, CancellationToken cancellationToken)
    {
        try
        {
            var profile = await profileService.CreateProfileAsync(request, cancellationToken);
            return CreatedAtAction(nameof(GetProfileById), new { id = profile.Id }, profile);
        }
        catch (ArgumentException ex)
        {
            return BadRequest(new { message = ex.Message });
        }
        catch (InvalidOperationException ex)
        {
            return BadRequest(new { message = ex.Message });
        }
    }

    [HttpPut("{id:guid}")]
    public async Task<IActionResult> UpdateProfile(Guid id, [FromBody] UpdateProfileRequest request, CancellationToken cancellationToken)
    {
        try
        {
            var profile = await profileService.UpdateProfileAsync(id, request, cancellationToken);
            return profile is null ? NotFound() : Ok(profile);
        }
        catch (ArgumentException ex)
        {
            return BadRequest(new { message = ex.Message });
        }
    }

    [HttpDelete("{id:guid}")]
    public async Task<IActionResult> DeleteProfile(Guid id, CancellationToken cancellationToken)
    {
        var deleted = await profileService.DeleteProfileAsync(id, cancellationToken);
        return deleted ? NoContent() : NotFound();
    }
}
