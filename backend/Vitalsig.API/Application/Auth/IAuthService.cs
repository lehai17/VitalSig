using Vitalsig.API.Application.Auth.Contracts;

namespace Vitalsig.API.Application.Auth;

public interface IAuthService
{
    Task<AuthResponse> RegisterAsync(RegisterRequest request, CancellationToken cancellationToken);
    Task<AuthResponse> LoginAsync(LoginRequest request, CancellationToken cancellationToken);
    Task<UserSummaryResponse?> GetCurrentUserAsync(Guid userId, CancellationToken cancellationToken);
}
