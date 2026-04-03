namespace Vitalsig.API.Application.QrCodes.Contracts;

public class QrCodeResponse
{
    public Guid Id { get; set; }
    public Guid ProfileId { get; set; }
    public string PublicToken { get; set; } = string.Empty;
    public string PublicUrl { get; set; } = string.Empty;
    public string QrImageDataUrl { get; set; } = string.Empty;
    public bool IsActive { get; set; }
    public DateTime GeneratedAtUtc { get; set; }
    public DateTime? ExpiredAtUtc { get; set; }
}
