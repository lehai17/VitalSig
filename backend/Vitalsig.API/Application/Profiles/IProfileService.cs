using Vitalsig.API.Application.Profiles.Contracts;

namespace Vitalsig.API.Application.Profiles;

public interface IProfileService
{
    Task<IReadOnlyList<ProfileListItemResponse>> GetProfilesAsync(Guid? ownerUserId, CancellationToken cancellationToken);
    Task<ProfileDetailResponse?> GetProfileByIdAsync(Guid id, CancellationToken cancellationToken);
    Task<ProfileDetailResponse> CreateProfileAsync(CreateProfileRequest request, CancellationToken cancellationToken);
    Task<ProfileDetailResponse?> UpdateProfileAsync(Guid id, UpdateProfileRequest request, CancellationToken cancellationToken);
    Task<bool> DeleteProfileAsync(Guid id, CancellationToken cancellationToken);
}
