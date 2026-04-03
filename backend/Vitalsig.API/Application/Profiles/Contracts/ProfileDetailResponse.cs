using Vitalsig.API.Domain.Enums;

namespace Vitalsig.API.Application.Profiles.Contracts;

public class ProfileDetailResponse
{
    public Guid Id { get; set; }
    public Guid OwnerUserId { get; set; }
    public string ProfileCode { get; set; } = string.Empty;
    public string PublicToken { get; set; } = string.Empty;
    public ProfileType ProfileType { get; set; }
    public string DisplayName { get; set; } = string.Empty;
    public DateOnly? DateOfBirth { get; set; }
    public Gender Gender { get; set; }
    public string? AvatarUrl { get; set; }
    public string? IdentificationNote { get; set; }
    public string? AddressNote { get; set; }
    public bool IsPublic { get; set; }
    public bool IsActive { get; set; }
    public DateTime CreatedAtUtc { get; set; }
    public DateTime? UpdatedAtUtc { get; set; }
    public List<EmergencyContactResponse> EmergencyContacts { get; set; } = [];
    public MedicalInfoResponse? MedicalInfo { get; set; }
    public ProfileAccessSettingResponse? AccessSetting { get; set; }
}
