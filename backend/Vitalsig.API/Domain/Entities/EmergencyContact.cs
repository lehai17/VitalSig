namespace Vitalsig.API.Domain.Entities;

public class EmergencyContact
{
    public Guid Id { get; set; }
    public Guid ProfileId { get; set; }
    public string FullName { get; set; } = string.Empty;
    public string Relationship { get; set; } = string.Empty;
    public string PhoneNumber { get; set; } = string.Empty;
    public int Priority { get; set; } = 1;
    public bool IsPrimary { get; set; } = false;
    public DateTime CreatedAtUtc { get; set; } = DateTime.UtcNow;

    public Profile Profile { get; set; } = null!;
}
