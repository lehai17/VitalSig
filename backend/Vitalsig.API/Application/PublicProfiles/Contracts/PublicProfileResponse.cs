using Vitalsig.API.Domain.Enums;

namespace Vitalsig.API.Application.PublicProfiles.Contracts;

public class PublicProfileResponse
{
    public Guid Id { get; set; }
    public string PublicToken { get; set; } = string.Empty;
    public string? DisplayName { get; set; }
    public string? AvatarUrl { get; set; }
    public ProfileType ProfileType { get; set; }
    public Gender Gender { get; set; }
    public string? IdentificationNote { get; set; }
    public string? AddressNote { get; set; }
    public bool IsPublic { get; set; }
    public bool CanShowMedicalInfo { get; set; }
    public bool CanShowEmergencyContacts { get; set; }
    public List<PublicEmergencyContactResponse> EmergencyContacts { get; set; } = [];
    public PublicMedicalInfoResponse? MedicalInfo { get; set; }
}
