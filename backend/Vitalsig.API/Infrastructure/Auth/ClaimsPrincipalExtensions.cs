using System.Security.Claims;

namespace Vitalsig.API.Infrastructure.Auth;

public static class ClaimsPrincipalExtensions
{
    public static Guid? GetUserId(this ClaimsPrincipal user)
    {
        var rawValue = user.FindFirstValue(ClaimTypes.NameIdentifier)
                       ?? user.FindFirstValue(ClaimTypes.Name)
                       ?? user.FindFirstValue("sub");

        return Guid.TryParse(rawValue, out var userId) ? userId : null;
    }
}
