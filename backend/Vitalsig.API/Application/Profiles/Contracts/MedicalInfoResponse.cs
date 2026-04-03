namespace Vitalsig.API.Application.Profiles.Contracts;

public class MedicalInfoResponse
{
    public Guid Id { get; set; }
    public string? BloodType { get; set; }
    public string? ChronicDiseases { get; set; }
    public string? Allergies { get; set; }
    public string? CurrentMedications { get; set; }
    public string? EmergencyInstructions { get; set; }
    public string? DoctorName { get; set; }
    public string? DoctorPhone { get; set; }
    public DateTime? UpdatedAtUtc { get; set; }
}
