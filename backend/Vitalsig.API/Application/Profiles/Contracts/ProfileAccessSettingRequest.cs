namespace Vitalsig.API.Application.Profiles.Contracts;

public class ProfileAccessSettingRequest
{
    public bool ShowFullName { get; set; } = true;
    public bool ShowPhoto { get; set; } = true;
    public bool ShowMedicalInfo { get; set; } = true;
    public bool ShowEmergencyContacts { get; set; } = true;
    public bool ShowAddressNote { get; set; }
    public bool AllowScanLogging { get; set; } = true;
}
