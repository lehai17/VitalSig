namespace Vitalsig.API.Application.Profiles.Contracts;

public class EmergencyContactRequest
{
    public string FullName { get; set; } = string.Empty;
    public string Relationship { get; set; } = string.Empty;
    public string PhoneNumber { get; set; } = string.Empty;
    public int Priority { get; set; } = 1;
    public bool IsPrimary { get; set; }
}
