using Vitalsig.API.Application.Auth.Contracts;
using Vitalsig.API.Domain.Entities;

namespace Vitalsig.API.Infrastructure.Auth;

public interface IJwtTokenService
{
    AuthResponse CreateToken(User user);
}
