using Vitalsig.API.Domain.Enums;

namespace Vitalsig.API.Application.Profiles.Contracts;

public class ProfileListItemResponse
{
    public Guid Id { get; set; }
    public Guid OwnerUserId { get; set; }
    public string ProfileCode { get; set; } = string.Empty;
    public string PublicToken { get; set; } = string.Empty;
    public ProfileType ProfileType { get; set; }
    public string DisplayName { get; set; } = string.Empty;
    public bool IsPublic { get; set; }
    public bool IsActive { get; set; }
    public DateTime CreatedAtUtc { get; set; }
    public int EmergencyContactCount { get; set; }
}
