using Vitalsig.API.Application.ScanLogs.Contracts;

namespace Vitalsig.API.Application.ScanLogs;

public interface IProfileScanLogService
{
    Task<IReadOnlyList<ProfileScanLogListItemResponse>> GetProfileScanLogsAsync(
        Guid profileId,
        Guid ownerUserId,
        CancellationToken cancellationToken);
}
