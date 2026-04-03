using Microsoft.EntityFrameworkCore;
using Vitalsig.API.Application.Profiles.Contracts;
using Vitalsig.API.Domain.Entities;
using Vitalsig.API.Infrastructure.Data;

namespace Vitalsig.API.Application.Profiles;

public class ProfileService(AppDbContext dbContext) : IProfileService
{
    public async Task<IReadOnlyList<ProfileListItemResponse>> GetProfilesAsync(Guid ownerUserId, CancellationToken cancellationToken)
    {
        var query = dbContext.Profiles
            .AsNoTracking()
            .Include(x => x.EmergencyContacts)
            .Where(x => x.OwnerUserId == ownerUserId);

        return await query
            .OrderByDescending(x => x.CreatedAtUtc)
            .Select(x => new ProfileListItemResponse
            {
                Id = x.Id,
                OwnerUserId = x.OwnerUserId,
                ProfileCode = x.ProfileCode,
                PublicToken = x.PublicToken,
                ProfileType = x.ProfileType,
                DisplayName = x.DisplayName,
                IsPublic = x.IsPublic,
                IsActive = x.IsActive,
                CreatedAtUtc = x.CreatedAtUtc,
                EmergencyContactCount = x.EmergencyContacts.Count
            })
            .ToListAsync(cancellationToken);
    }

    public async Task<ProfileDetailResponse?> GetProfileByIdAsync(Guid id, Guid ownerUserId, CancellationToken cancellationToken)
    {
        var profile = await dbContext.Profiles
            .AsNoTracking()
            .Include(x => x.EmergencyContacts)
            .Include(x => x.MedicalInfo)
            .Include(x => x.AccessSetting)
            .FirstOrDefaultAsync(x => x.Id == id && x.OwnerUserId == ownerUserId, cancellationToken);

        return profile is null ? null : MapToDetailResponse(profile);
    }

    public async Task<ProfileDetailResponse> CreateProfileAsync(Guid ownerUserId, CreateProfileRequest request, CancellationToken cancellationToken)
    {
        ValidateProfileRequest(request.DisplayName, request.EmergencyContacts);

        var ownerExists = await dbContext.Users
            .AsNoTracking()
            .AnyAsync(x => x.Id == ownerUserId, cancellationToken);

        if (!ownerExists)
        {
            throw new InvalidOperationException("Owner user does not exist.");
        }

        var profile = new Profile
        {
            Id = Guid.NewGuid(),
            OwnerUserId = ownerUserId,
            ProfileCode = GenerateProfileCode(),
            PublicToken = GeneratePublicToken(),
            ProfileType = request.ProfileType,
            DisplayName = request.DisplayName.Trim(),
            DateOfBirth = request.DateOfBirth,
            Gender = request.Gender,
            AvatarUrl = request.AvatarUrl?.Trim(),
            IdentificationNote = request.IdentificationNote?.Trim(),
            AddressNote = request.AddressNote?.Trim(),
            IsPublic = request.IsPublic,
            IsActive = request.IsActive,
            CreatedAtUtc = DateTime.UtcNow,
            EmergencyContacts = request.EmergencyContacts
                .Select(contact => MapEmergencyContact(contact, profileId: Guid.Empty))
                .ToList(),
            AccessSetting = MapAccessSetting(request.AccessSetting),
            MedicalInfo = MapMedicalInfo(request.MedicalInfo)
        };

        foreach (var emergencyContact in profile.EmergencyContacts)
        {
            emergencyContact.ProfileId = profile.Id;
        }

        if (profile.MedicalInfo is not null)
        {
            profile.MedicalInfo.ProfileId = profile.Id;
        }

        if (profile.AccessSetting is not null)
        {
            profile.AccessSetting.ProfileId = profile.Id;
        }

        dbContext.Profiles.Add(profile);
        await dbContext.SaveChangesAsync(cancellationToken);

        return MapToDetailResponse(profile);
    }

    public async Task<ProfileDetailResponse?> UpdateProfileAsync(Guid id, Guid ownerUserId, UpdateProfileRequest request, CancellationToken cancellationToken)
    {
        ValidateProfileRequest(request.DisplayName, request.EmergencyContacts);

        var profile = await dbContext.Profiles
            .Include(x => x.EmergencyContacts)
            .Include(x => x.MedicalInfo)
            .Include(x => x.AccessSetting)
            .FirstOrDefaultAsync(x => x.Id == id && x.OwnerUserId == ownerUserId, cancellationToken);

        if (profile is null)
        {
            return null;
        }

        profile.ProfileType = request.ProfileType;
        profile.DisplayName = request.DisplayName.Trim();
        profile.DateOfBirth = request.DateOfBirth;
        profile.Gender = request.Gender;
        profile.AvatarUrl = request.AvatarUrl?.Trim();
        profile.IdentificationNote = request.IdentificationNote?.Trim();
        profile.AddressNote = request.AddressNote?.Trim();
        profile.IsPublic = request.IsPublic;
        profile.IsActive = request.IsActive;
        profile.UpdatedAtUtc = DateTime.UtcNow;

        dbContext.EmergencyContacts.RemoveRange(profile.EmergencyContacts);
        profile.EmergencyContacts = request.EmergencyContacts
            .Select(contact => MapEmergencyContact(contact, profile.Id))
            .ToList();

        if (request.MedicalInfo is null)
        {
            if (profile.MedicalInfo is not null)
            {
                dbContext.MedicalInfos.Remove(profile.MedicalInfo);
                profile.MedicalInfo = null;
            }
        }
        else if (profile.MedicalInfo is null)
        {
            profile.MedicalInfo = MapMedicalInfo(request.MedicalInfo, profileId: profile.Id);
        }
        else
        {
            profile.MedicalInfo.BloodType = request.MedicalInfo.BloodType?.Trim();
            profile.MedicalInfo.ChronicDiseases = request.MedicalInfo.ChronicDiseases?.Trim();
            profile.MedicalInfo.Allergies = request.MedicalInfo.Allergies?.Trim();
            profile.MedicalInfo.CurrentMedications = request.MedicalInfo.CurrentMedications?.Trim();
            profile.MedicalInfo.EmergencyInstructions = request.MedicalInfo.EmergencyInstructions?.Trim();
            profile.MedicalInfo.DoctorName = request.MedicalInfo.DoctorName?.Trim();
            profile.MedicalInfo.DoctorPhone = request.MedicalInfo.DoctorPhone?.Trim();
            profile.MedicalInfo.UpdatedAtUtc = DateTime.UtcNow;
        }

        if (profile.AccessSetting is null)
        {
            profile.AccessSetting = MapAccessSetting(request.AccessSetting, profileId: profile.Id);
        }
        else
        {
            var accessSettingRequest = request.AccessSetting ?? new ProfileAccessSettingRequest();
            profile.AccessSetting.ShowFullName = accessSettingRequest.ShowFullName;
            profile.AccessSetting.ShowPhoto = accessSettingRequest.ShowPhoto;
            profile.AccessSetting.ShowMedicalInfo = accessSettingRequest.ShowMedicalInfo;
            profile.AccessSetting.ShowEmergencyContacts = accessSettingRequest.ShowEmergencyContacts;
            profile.AccessSetting.ShowAddressNote = accessSettingRequest.ShowAddressNote;
            profile.AccessSetting.AllowScanLogging = accessSettingRequest.AllowScanLogging;
        }

        await dbContext.SaveChangesAsync(cancellationToken);

        return MapToDetailResponse(profile);
    }

    public async Task<bool> DeleteProfileAsync(Guid id, Guid ownerUserId, CancellationToken cancellationToken)
    {
        var profile = await dbContext.Profiles.FirstOrDefaultAsync(
            x => x.Id == id && x.OwnerUserId == ownerUserId,
            cancellationToken);
        if (profile is null)
        {
            return false;
        }

        dbContext.Profiles.Remove(profile);
        await dbContext.SaveChangesAsync(cancellationToken);

        return true;
    }

    private static void ValidateProfileRequest(string displayName, List<EmergencyContactRequest> emergencyContacts)
    {
        if (string.IsNullOrWhiteSpace(displayName))
        {
            throw new ArgumentException("Display name is required.");
        }

        if (emergencyContacts.Count == 0)
        {
            throw new ArgumentException("At least one emergency contact is required.");
        }

        if (emergencyContacts.Any(x =>
                string.IsNullOrWhiteSpace(x.FullName) ||
                string.IsNullOrWhiteSpace(x.Relationship) ||
                string.IsNullOrWhiteSpace(x.PhoneNumber)))
        {
            throw new ArgumentException("Emergency contact name, relationship, and phone number are required.");
        }
    }

    private static EmergencyContact MapEmergencyContact(EmergencyContactRequest request, Guid profileId)
    {
        return new EmergencyContact
        {
            Id = Guid.NewGuid(),
            ProfileId = profileId,
            FullName = request.FullName.Trim(),
            Relationship = request.Relationship.Trim(),
            PhoneNumber = request.PhoneNumber.Trim(),
            Priority = request.Priority,
            IsPrimary = request.IsPrimary,
            CreatedAtUtc = DateTime.UtcNow
        };
    }

    private static MedicalInfo? MapMedicalInfo(MedicalInfoRequest? request, Guid? id = null, Guid? profileId = null)
    {
        if (request is null)
        {
            return null;
        }

        return new MedicalInfo
        {
            Id = id ?? Guid.NewGuid(),
            ProfileId = profileId ?? Guid.Empty,
            BloodType = request.BloodType?.Trim(),
            ChronicDiseases = request.ChronicDiseases?.Trim(),
            Allergies = request.Allergies?.Trim(),
            CurrentMedications = request.CurrentMedications?.Trim(),
            EmergencyInstructions = request.EmergencyInstructions?.Trim(),
            DoctorName = request.DoctorName?.Trim(),
            DoctorPhone = request.DoctorPhone?.Trim(),
            UpdatedAtUtc = DateTime.UtcNow
        };
    }

    private static ProfileAccessSetting MapAccessSetting(ProfileAccessSettingRequest? request, Guid? id = null, Guid? profileId = null)
    {
        request ??= new ProfileAccessSettingRequest();

        return new ProfileAccessSetting
        {
            Id = id ?? Guid.NewGuid(),
            ProfileId = profileId ?? Guid.Empty,
            ShowFullName = request.ShowFullName,
            ShowPhoto = request.ShowPhoto,
            ShowMedicalInfo = request.ShowMedicalInfo,
            ShowEmergencyContacts = request.ShowEmergencyContacts,
            ShowAddressNote = request.ShowAddressNote,
            AllowScanLogging = request.AllowScanLogging
        };
    }

    private static ProfileDetailResponse MapToDetailResponse(Profile profile)
    {
        return new ProfileDetailResponse
        {
            Id = profile.Id,
            OwnerUserId = profile.OwnerUserId,
            ProfileCode = profile.ProfileCode,
            PublicToken = profile.PublicToken,
            ProfileType = profile.ProfileType,
            DisplayName = profile.DisplayName,
            DateOfBirth = profile.DateOfBirth,
            Gender = profile.Gender,
            AvatarUrl = profile.AvatarUrl,
            IdentificationNote = profile.IdentificationNote,
            AddressNote = profile.AddressNote,
            IsPublic = profile.IsPublic,
            IsActive = profile.IsActive,
            CreatedAtUtc = profile.CreatedAtUtc,
            UpdatedAtUtc = profile.UpdatedAtUtc,
            EmergencyContacts = profile.EmergencyContacts
                .OrderBy(x => x.Priority)
                .Select(x => new EmergencyContactResponse
                {
                    Id = x.Id,
                    FullName = x.FullName,
                    Relationship = x.Relationship,
                    PhoneNumber = x.PhoneNumber,
                    Priority = x.Priority,
                    IsPrimary = x.IsPrimary
                })
                .ToList(),
            MedicalInfo = profile.MedicalInfo is null
                ? null
                : new MedicalInfoResponse
                {
                    Id = profile.MedicalInfo.Id,
                    BloodType = profile.MedicalInfo.BloodType,
                    ChronicDiseases = profile.MedicalInfo.ChronicDiseases,
                    Allergies = profile.MedicalInfo.Allergies,
                    CurrentMedications = profile.MedicalInfo.CurrentMedications,
                    EmergencyInstructions = profile.MedicalInfo.EmergencyInstructions,
                    DoctorName = profile.MedicalInfo.DoctorName,
                    DoctorPhone = profile.MedicalInfo.DoctorPhone,
                    UpdatedAtUtc = profile.MedicalInfo.UpdatedAtUtc
                },
            AccessSetting = profile.AccessSetting is null
                ? null
                : new ProfileAccessSettingResponse
                {
                    Id = profile.AccessSetting.Id,
                    ShowFullName = profile.AccessSetting.ShowFullName,
                    ShowPhoto = profile.AccessSetting.ShowPhoto,
                    ShowMedicalInfo = profile.AccessSetting.ShowMedicalInfo,
                    ShowEmergencyContacts = profile.AccessSetting.ShowEmergencyContacts,
                    ShowAddressNote = profile.AccessSetting.ShowAddressNote,
                    AllowScanLogging = profile.AccessSetting.AllowScanLogging
                }
        };
    }

    private static string GenerateProfileCode()
    {
        return $"VS-{DateTime.UtcNow:yyyyMMdd}-{Random.Shared.Next(1000, 9999)}";
    }

    private static string GeneratePublicToken()
    {
        return Guid.NewGuid().ToString("N");
    }
}
