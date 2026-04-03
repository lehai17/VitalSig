using Vitalsig.API.Domain.Enums;

namespace Vitalsig.API.Application.Profiles.Contracts;

public class CreateProfileRequest
{
    public Guid OwnerUserId { get; set; }
    public ProfileType ProfileType { get; set; } = ProfileType.Combined;
    public string DisplayName { get; set; } = string.Empty;
    public DateOnly? DateOfBirth { get; set; }
    public Gender Gender { get; set; } = Gender.Unknown;
    public string? AvatarUrl { get; set; }
    public string? IdentificationNote { get; set; }
    public string? AddressNote { get; set; }
    public bool IsPublic { get; set; } = true;
    public bool IsActive { get; set; } = true;
    public List<EmergencyContactRequest> EmergencyContacts { get; set; } = [];
    public MedicalInfoRequest? MedicalInfo { get; set; }
    public ProfileAccessSettingRequest? AccessSetting { get; set; }
}
