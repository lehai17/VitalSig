namespace Vitalsig.API.Application.PublicProfiles.Contracts;

public class PublicEmergencyContactResponse
{
    public string FullName { get; set; } = string.Empty;
    public string Relationship { get; set; } = string.Empty;
    public string PhoneNumber { get; set; } = string.Empty;
    public int Priority { get; set; }
    public bool IsPrimary { get; set; }
}
