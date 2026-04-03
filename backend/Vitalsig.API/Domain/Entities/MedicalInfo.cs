namespace Vitalsig.API.Domain.Entities;

public class MedicalInfo
{
    public Guid Id { get; set; }
    public Guid ProfileId { get; set; }
    public string? BloodType { get; set; }
    public string? ChronicDiseases { get; set; }
    public string? Allergies { get; set; }
    public string? CurrentMedications { get; set; }
    public string? EmergencyInstructions { get; set; }
    public string? DoctorName { get; set; }
    public string? DoctorPhone { get; set; }
    public DateTime? UpdatedAtUtc { get; set; }

    public Profile Profile { get; set; } = null!;
}
