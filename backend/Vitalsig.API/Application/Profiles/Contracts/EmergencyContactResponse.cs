namespace Vitalsig.API.Application.Profiles.Contracts;

public class EmergencyContactResponse
{
    public Guid Id { get; set; }
    public string FullName { get; set; } = string.Empty;
    public string Relationship { get; set; } = string.Empty;
    public string PhoneNumber { get; set; } = string.Empty;
    public int Priority { get; set; }
    public bool IsPrimary { get; set; }
}
