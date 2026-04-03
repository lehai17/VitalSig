using Vitalsig.API.Application.QrCodes.Contracts;

namespace Vitalsig.API.Application.QrCodes;

public interface IQrCodeService
{
    Task<QrCodeResponse?> GetActiveQrCodeAsync(Guid profileId, string publicUrlBase, CancellationToken cancellationToken);
    Task<QrCodeResponse?> RegenerateQrCodeAsync(Guid profileId, string publicUrlBase, CancellationToken cancellationToken);
}
