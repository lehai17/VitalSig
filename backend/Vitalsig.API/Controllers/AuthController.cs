using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Vitalsig.API.Application.Auth;
using Vitalsig.API.Application.Auth.Contracts;
using Vitalsig.API.Infrastructure.Auth;

namespace Vitalsig.API.Controllers;

[ApiController]
[AllowAnonymous]
[Route("api/auth")]
public class AuthController(IAuthService authService) : ControllerBase
{
    [Authorize]
    [HttpGet("me")]
    public async Task<IActionResult> Me(CancellationToken cancellationToken)
    {
        var currentUserId = User.GetUserId();
        if (!currentUserId.HasValue)
        {
            return Unauthorized();
        }

        var user = await authService.GetCurrentUserAsync(currentUserId.Value, cancellationToken);
        return user is null ? Unauthorized() : Ok(user);
    }

    [HttpPost("register")]
    public async Task<IActionResult> Register([FromBody] RegisterRequest request, CancellationToken cancellationToken)
    {
        try
        {
            var response = await authService.RegisterAsync(request, cancellationToken);
            return Ok(response);
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

    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] LoginRequest request, CancellationToken cancellationToken)
    {
        try
        {
            var response = await authService.LoginAsync(request, cancellationToken);
            return Ok(response);
        }
        catch (ArgumentException ex)
        {
            return BadRequest(new { message = ex.Message });
        }
        catch (UnauthorizedAccessException ex)
        {
            return Unauthorized(new { message = ex.Message });
        }
    }
}
