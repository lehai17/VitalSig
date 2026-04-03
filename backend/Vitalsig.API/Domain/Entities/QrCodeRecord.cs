namespace Vitalsig.API.Domain.Entities;

public class QrCodeRecord
{
    public Guid Id { get; set; }
    public Guid ProfileId { get; set; }
    public string QrValue { get; set; } = string.Empty;
    public string? QrImageUrl { get; set; }
    public bool IsActive { get; set; } = true;
    public DateTime GeneratedAtUtc { get; set; } = DateTime.UtcNow;
    public DateTime? ExpiredAtUtc { get; set; }

    public Profile Profile { get; set; } = null!;
}
