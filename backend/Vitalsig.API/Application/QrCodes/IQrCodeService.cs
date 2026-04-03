using Vitalsig.API.Application.QrCodes.Contracts;

namespace Vitalsig.API.Application.QrCodes;

public interface IQrCodeService
{
    Task<QrCodeResponse?> GetActiveQrCodeAsync(OwnedQrCodeRequest request, CancellationToken cancellationToken);
    Task<QrCodeResponse?> RegenerateQrCodeAsync(OwnedQrCodeRequest request, CancellationToken cancellationToken);
}
