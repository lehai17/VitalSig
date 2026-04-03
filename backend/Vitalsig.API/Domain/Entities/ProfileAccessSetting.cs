namespace Vitalsig.API.Domain.Entities;

public class ProfileAccessSetting
{
    public Guid Id { get; set; }
    public Guid ProfileId { get; set; }
    public bool ShowFullName { get; set; } = true;
    public bool ShowPhoto { get; set; } = true;
    public bool ShowMedicalInfo { get; set; } = true;
    public bool ShowEmergencyContacts { get; set; } = true;
    public bool ShowAddressNote { get; set; } = false;
    public bool AllowScanLogging { get; set; } = true;

    public Profile Profile { get; set; } = null!;
}
