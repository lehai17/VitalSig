namespace Vitalsig.API.Application.Profiles.Contracts;

public class ProfileAccessSettingResponse
{
    public Guid Id { get; set; }
    public bool ShowFullName { get; set; }
    public bool ShowPhoto { get; set; }
    public bool ShowMedicalInfo { get; set; }
    public bool ShowEmergencyContacts { get; set; }
    public bool ShowAddressNote { get; set; }
    public bool AllowScanLogging { get; set; }
}
