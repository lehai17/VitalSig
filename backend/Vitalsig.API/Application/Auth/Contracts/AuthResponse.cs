namespace Vitalsig.API.Application.Auth.Contracts;

public class AuthResponse
{
    public string Token { get; set; } = string.Empty;
    public DateTime ExpiresAtUtc { get; set; }
    public UserSummaryResponse User { get; set; } = new();
}
