using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Vitalsig.API.Application.QrCodes;
using Vitalsig.API.Application.QrCodes.Contracts;
using Vitalsig.API.Infrastructure.Auth;

namespace Vitalsig.API.Controllers;

[ApiController]
[Authorize]
[Route("api/profiles/{profileId:guid}/qr")]
public class ProfileQrCodesController(IQrCodeService qrCodeService) : ControllerBase
{
    [HttpGet]
    public async Task<IActionResult> GetActiveQrCode(Guid profileId, CancellationToken cancellationToken)
    {
        var currentUserId = User.GetUserId();
        if (!currentUserId.HasValue)
        {
            return Unauthorized();
        }

        var qrCode = await qrCodeService.GetActiveQrCodeAsync(
            new OwnedQrCodeRequest
            {
                ProfileId = profileId,
                OwnerUserId = currentUserId.Value,
                PublicUrlBase = BuildPublicUrlBase()
            },
            cancellationToken);

        return qrCode is null ? NotFound() : Ok(qrCode);
    }

    [HttpPost("regenerate")]
    public async Task<IActionResult> RegenerateQrCode(Guid profileId, CancellationToken cancellationToken)
    {
        var currentUserId = User.GetUserId();
        if (!currentUserId.HasValue)
        {
            return Unauthorized();
        }

        var qrCode = await qrCodeService.RegenerateQrCodeAsync(
            new OwnedQrCodeRequest
            {
                ProfileId = profileId,
                OwnerUserId = currentUserId.Value,
                PublicUrlBase = BuildPublicUrlBase()
            },
            cancellationToken);

        return qrCode is null ? NotFound() : Ok(qrCode);
    }

    private string BuildPublicUrlBase()
    {
        return $"{Request.Scheme}://{Request.Host}/api/public/p";
    }
}
