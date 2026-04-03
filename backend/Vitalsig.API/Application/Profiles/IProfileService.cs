using Vitalsig.API.Application.Profiles.Contracts;

namespace Vitalsig.API.Application.Profiles;

public interface IProfileService
{
    Task<IReadOnlyList<ProfileListItemResponse>> GetProfilesAsync(Guid ownerUserId, CancellationToken cancellationToken);
    Task<ProfileDetailResponse?> GetProfileByIdAsync(Guid id, Guid ownerUserId, CancellationToken cancellationToken);
    Task<ProfileDetailResponse> CreateProfileAsync(Guid ownerUserId, CreateProfileRequest request, CancellationToken cancellationToken);
    Task<ProfileDetailResponse?> UpdateProfileAsync(Guid id, Guid ownerUserId, UpdateProfileRequest request, CancellationToken cancellationToken);
    Task<bool> DeleteProfileAsync(Guid id, Guid ownerUserId, CancellationToken cancellationToken);
}
