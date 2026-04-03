namespace Vitalsig.API.Application.ScanLogs.Contracts;

public class ProfileScanLogListItemResponse
{
    public Guid Id { get; set; }
    public Guid ProfileId { get; set; }
    public DateTime ScannedAtUtc { get; set; }
    public string? IpAddress { get; set; }
    public string? UserAgent { get; set; }
    public string? LocationText { get; set; }
    public decimal? Latitude { get; set; }
    public decimal? Longitude { get; set; }
    public string? ActionType { get; set; }
    public string? Note { get; set; }
}
