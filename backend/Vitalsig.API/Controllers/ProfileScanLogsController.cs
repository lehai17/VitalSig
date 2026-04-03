using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Vitalsig.API.Application.ScanLogs;
using Vitalsig.API.Infrastructure.Auth;

namespace Vitalsig.API.Controllers;

[ApiController]
[Authorize]
[Route("api/profiles/{profileId:guid}/scan-logs")]
public class ProfileScanLogsController(IProfileScanLogService profileScanLogService) : ControllerBase
{
    [HttpGet]
    public async Task<IActionResult> GetProfileScanLogs(Guid profileId, CancellationToken cancellationToken)
    {
        var currentUserId = User.GetUserId();
        if (!currentUserId.HasValue)
        {
            return Unauthorized();
        }

        var scanLogs = await profileScanLogService.GetProfileScanLogsAsync(
            profileId,
            currentUserId.Value,
            cancellationToken);

        return Ok(scanLogs);
    }
}
