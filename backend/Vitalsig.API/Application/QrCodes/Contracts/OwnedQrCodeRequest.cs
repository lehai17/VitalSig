namespace Vitalsig.API.Application.QrCodes.Contracts;

public class OwnedQrCodeRequest
{
    public Guid ProfileId { get; set; }
    public Guid OwnerUserId { get; set; }
    public string PublicUrlBase { get; set; } = string.Empty;
}
