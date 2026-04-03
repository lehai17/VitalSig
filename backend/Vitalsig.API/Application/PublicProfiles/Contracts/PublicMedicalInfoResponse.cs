namespace Vitalsig.API.Application.PublicProfiles.Contracts;

public class PublicMedicalInfoResponse
{
    public string? BloodType { get; set; }
    public string? ChronicDiseases { get; set; }
    public string? Allergies { get; set; }
    public string? CurrentMedications { get; set; }
    public string? EmergencyInstructions { get; set; }
}
