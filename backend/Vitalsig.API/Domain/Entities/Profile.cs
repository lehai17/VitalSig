using Vitalsig.API.Domain.Enums;

namespace Vitalsig.API.Domain.Entities;

public class Profile
{
    public Guid Id { get; set; }
    public Guid OwnerUserId { get; set; }
    public string ProfileCode { get; set; } = string.Empty;
    public string PublicToken { get; set; } = string.Empty;
    public ProfileType ProfileType { get; set; } = ProfileType.Combined;
    public string DisplayName { get; set; } = string.Empty;
    public DateOnly? DateOfBirth { get; set; }
    public Gender Gender { get; set; } = Gender.Unknown;
    public string? AvatarUrl { get; set; }
    public string? IdentificationNote { get; set; }
    public string? AddressNote { get; set; }
    public bool IsPublic { get; set; } = true;
    public bool IsActive { get; set; } = true;
    public DateTime CreatedAtUtc { get; set; } = DateTime.UtcNow;
    public DateTime? UpdatedAtUtc { get; set; }

    public User OwnerUser { get; set; } = null!;
    public MedicalInfo? MedicalInfo { get; set; }
    public ProfileAccessSetting? AccessSetting { get; set; }
    public ICollection<EmergencyContact> EmergencyContacts { get; set; } = new List<EmergencyContact>();
    public ICollection<QrCodeRecord> QrCodes { get; set; } = new List<QrCodeRecord>();
    public ICollection<ScanLog> ScanLogs { get; set; } = new List<ScanLog>();
}
