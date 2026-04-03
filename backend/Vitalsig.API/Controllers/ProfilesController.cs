using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Vitalsig.API.Application.Profiles;
using Vitalsig.API.Application.Profiles.Contracts;
using Vitalsig.API.Infrastructure.Auth;

namespace Vitalsig.API.Controllers;

[ApiController]
[Authorize]
[Route("api/profiles")]
public class ProfilesController(IProfileService profileService) : ControllerBase
{
    [HttpGet]
    public async Task<IActionResult> GetProfiles(CancellationToken cancellationToken)
    {
        var currentUserId = User.GetUserId();
        if (!currentUserId.HasValue)
        {
            return Unauthorized();
        }

        var profiles = await profileService.GetProfilesAsync(currentUserId.Value, cancellationToken);
        return Ok(profiles);
    }

    [HttpGet("{id:guid}")]
    public async Task<IActionResult> GetProfileById(Guid id, CancellationToken cancellationToken)
    {
        var currentUserId = User.GetUserId();
        if (!currentUserId.HasValue)
        {
            return Unauthorized();
        }

        var profile = await profileService.GetProfileByIdAsync(id, currentUserId.Value, cancellationToken);
        return profile is null ? NotFound() : Ok(profile);
    }

    [HttpPost]
    public async Task<IActionResult> CreateProfile([FromBody] CreateProfileRequest request, CancellationToken cancellationToken)
    {
        try
        {
            var currentUserId = User.GetUserId();
            if (!currentUserId.HasValue)
            {
                return Unauthorized();
            }

            var profile = await profileService.CreateProfileAsync(currentUserId.Value, request, cancellationToken);
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
            var currentUserId = User.GetUserId();
            if (!currentUserId.HasValue)
            {
                return Unauthorized();
            }

            var profile = await profileService.UpdateProfileAsync(id, currentUserId.Value, request, cancellationToken);
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
        var currentUserId = User.GetUserId();
        if (!currentUserId.HasValue)
        {
            return Unauthorized();
        }

        var deleted = await profileService.DeleteProfileAsync(id, currentUserId.Value, cancellationToken);
        return deleted ? NoContent() : NotFound();
    }
}
