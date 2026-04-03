using Vitalsig.API.Application.PublicProfiles.Contracts;

namespace Vitalsig.API.Application.PublicProfiles;

public interface IPublicProfileService
{
    Task<PublicProfileResponse?> GetPublicProfileByTokenAsync(string token, CancellationToken cancellationToken);
    Task<ScanLogResponse?> CreateScanLogAsync(
        string token,
        CreateScanLogRequest request,
        string? ipAddress,
        string? userAgent,
        CancellationToken cancellationToken);
}
