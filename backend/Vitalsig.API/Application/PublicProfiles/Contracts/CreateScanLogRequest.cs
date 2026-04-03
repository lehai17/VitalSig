namespace Vitalsig.API.Application.PublicProfiles.Contracts;

public class CreateScanLogRequest
{
    public string? LocationText { get; set; }
    public decimal? Latitude { get; set; }
    public decimal? Longitude { get; set; }
    public string? ActionType { get; set; }
    public string? Note { get; set; }
}
