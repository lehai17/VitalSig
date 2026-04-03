using Microsoft.EntityFrameworkCore;
using Vitalsig.API.Application.ScanLogs.Contracts;
using Vitalsig.API.Infrastructure.Data;

namespace Vitalsig.API.Application.ScanLogs;

public class ProfileScanLogService(AppDbContext dbContext) : IProfileScanLogService
{
    public async Task<IReadOnlyList<ProfileScanLogListItemResponse>> GetProfileScanLogsAsync(
        Guid profileId,
        Guid ownerUserId,
        CancellationToken cancellationToken)
    {
        var profileExists = await dbContext.Profiles
            .AsNoTracking()
            .AnyAsync(x => x.Id == profileId && x.OwnerUserId == ownerUserId, cancellationToken);

        if (!profileExists)
        {
            return [];
        }

        return await dbContext.ScanLogs
            .AsNoTracking()
            .Where(x => x.ProfileId == profileId)
            .OrderByDescending(x => x.ScannedAtUtc)
            .Select(x => new ProfileScanLogListItemResponse
            {
                Id = x.Id,
                ProfileId = x.ProfileId,
                ScannedAtUtc = x.ScannedAtUtc,
                IpAddress = x.IpAddress,
                UserAgent = x.UserAgent,
                LocationText = x.LocationText,
                Latitude = x.Latitude,
                Longitude = x.Longitude,
                ActionType = x.ActionType,
                Note = x.Note
            })
            .ToListAsync(cancellationToken);
    }
}
