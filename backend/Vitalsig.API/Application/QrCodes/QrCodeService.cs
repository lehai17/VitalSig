using Microsoft.EntityFrameworkCore;
using QRCoder;
using Vitalsig.API.Application.QrCodes.Contracts;
using Vitalsig.API.Domain.Entities;
using Vitalsig.API.Infrastructure.Data;

namespace Vitalsig.API.Application.QrCodes;

public class QrCodeService(AppDbContext dbContext) : IQrCodeService
{
    public async Task<QrCodeResponse?> GetActiveQrCodeAsync(Guid profileId, string publicUrlBase, CancellationToken cancellationToken)
    {
        var profile = await dbContext.Profiles
            .AsNoTracking()
            .Include(x => x.QrCodes)
            .FirstOrDefaultAsync(x => x.Id == profileId, cancellationToken);

        if (profile is null)
        {
            return null;
        }

        var activeQrCode = profile.QrCodes
            .Where(x => x.IsActive)
            .OrderByDescending(x => x.GeneratedAtUtc)
            .FirstOrDefault();

        if (activeQrCode is null)
        {
            return await RegenerateQrCodeAsync(profileId, publicUrlBase, cancellationToken);
        }

        return MapToResponse(activeQrCode, profile.PublicToken, BuildPublicUrl(publicUrlBase, profile.PublicToken));
    }

    public async Task<QrCodeResponse?> RegenerateQrCodeAsync(Guid profileId, string publicUrlBase, CancellationToken cancellationToken)
    {
        var profile = await dbContext.Profiles
            .Include(x => x.QrCodes)
            .FirstOrDefaultAsync(x => x.Id == profileId, cancellationToken);

        if (profile is null)
        {
            return null;
        }

        foreach (var qrCode in profile.QrCodes.Where(x => x.IsActive))
        {
            qrCode.IsActive = false;
            qrCode.ExpiredAtUtc = DateTime.UtcNow;
        }

        var publicUrl = BuildPublicUrl(publicUrlBase, profile.PublicToken);
        var dataUrl = GenerateQrPngDataUrl(publicUrl);

        var qrCodeRecord = new QrCodeRecord
        {
            Id = Guid.NewGuid(),
            ProfileId = profile.Id,
            QrValue = publicUrl,
            QrImageUrl = dataUrl,
            IsActive = true,
            GeneratedAtUtc = DateTime.UtcNow
        };

        dbContext.QrCodes.Add(qrCodeRecord);
        await dbContext.SaveChangesAsync(cancellationToken);

        return MapToResponse(qrCodeRecord, profile.PublicToken, publicUrl);
    }

    private static QrCodeResponse MapToResponse(QrCodeRecord qrCodeRecord, string publicToken, string publicUrl)
    {
        return new QrCodeResponse
        {
            Id = qrCodeRecord.Id,
            ProfileId = qrCodeRecord.ProfileId,
            PublicToken = publicToken,
            PublicUrl = publicUrl,
            QrImageDataUrl = qrCodeRecord.QrImageUrl ?? string.Empty,
            IsActive = qrCodeRecord.IsActive,
            GeneratedAtUtc = qrCodeRecord.GeneratedAtUtc,
            ExpiredAtUtc = qrCodeRecord.ExpiredAtUtc
        };
    }

    private static string BuildPublicUrl(string publicUrlBase, string publicToken)
    {
        return $"{publicUrlBase.TrimEnd('/')}/{publicToken}";
    }

    private static string GenerateQrPngDataUrl(string content)
    {
        using var qrGenerator = new QRCodeGenerator();
        using var qrCodeData = qrGenerator.CreateQrCode(content, QRCodeGenerator.ECCLevel.Q);
        var pngQrCode = new PngByteQRCode(qrCodeData);
        var qrBytes = pngQrCode.GetGraphic(20);
        return $"data:image/png;base64,{Convert.ToBase64String(qrBytes)}";
    }
}
