using Microsoft.EntityFrameworkCore;
using Vitalsig.API.Application.PublicProfiles.Contracts;
using Vitalsig.API.Domain.Entities;
using Vitalsig.API.Infrastructure.Data;

namespace Vitalsig.API.Application.PublicProfiles;

public class PublicProfileService(AppDbContext dbContext) : IPublicProfileService
{
    public async Task<PublicProfileResponse?> GetPublicProfileByTokenAsync(string token, CancellationToken cancellationToken)
    {
        if (string.IsNullOrWhiteSpace(token))
        {
            return null;
        }

        var profile = await dbContext.Profiles
            .AsNoTracking()
            .Include(x => x.EmergencyContacts)
            .Include(x => x.MedicalInfo)
            .Include(x => x.AccessSetting)
            .FirstOrDefaultAsync(x => x.PublicToken == token && x.IsPublic && x.IsActive, cancellationToken);

        return profile is null ? null : MapToPublicResponse(profile);
    }

    public async Task<ScanLogResponse?> CreateScanLogAsync(
        string token,
        CreateScanLogRequest request,
        string? ipAddress,
        string? userAgent,
        CancellationToken cancellationToken)
    {
        if (string.IsNullOrWhiteSpace(token))
        {
            return null;
        }

        var profile = await dbContext.Profiles
            .Include(x => x.AccessSetting)
            .FirstOrDefaultAsync(x => x.PublicToken == token && x.IsPublic && x.IsActive, cancellationToken);

        if (profile is null)
        {
            return null;
        }

        if (profile.AccessSetting?.AllowScanLogging == false)
        {
            return new ScanLogResponse
            {
                ProfileId = profile.Id,
                ScannedAtUtc = DateTime.UtcNow,
                ActionType = request.ActionType?.Trim() ?? "Viewed"
            };
        }

        var scanLog = new ScanLog
        {
            Id = Guid.NewGuid(),
            ProfileId = profile.Id,
            ScannedAtUtc = DateTime.UtcNow,
            IpAddress = TrimOrNull(ipAddress, 45),
            UserAgent = TrimOrNull(userAgent, 1000),
            LocationText = TrimOrNull(request.LocationText, 255),
            Latitude = request.Latitude,
            Longitude = request.Longitude,
            ActionType = TrimOrNull(request.ActionType, 100) ?? "Viewed",
            Note = TrimOrNull(request.Note, 1000)
        };

        dbContext.ScanLogs.Add(scanLog);
        await dbContext.SaveChangesAsync(cancellationToken);

        return MapToScanLogResponse(scanLog);
    }

    private static PublicProfileResponse MapToPublicResponse(Profile profile)
    {
        var accessSetting = profile.AccessSetting ?? new ProfileAccessSetting();

        return new PublicProfileResponse
        {
            Id = profile.Id,
            PublicToken = profile.PublicToken,
            DisplayName = accessSetting.ShowFullName ? profile.DisplayName : null,
            AvatarUrl = accessSetting.ShowPhoto ? profile.AvatarUrl : null,
            ProfileType = profile.ProfileType,
            Gender = profile.Gender,
            IdentificationNote = profile.IdentificationNote,
            AddressNote = accessSetting.ShowAddressNote ? profile.AddressNote : null,
            IsPublic = profile.IsPublic,
            CanShowMedicalInfo = accessSetting.ShowMedicalInfo,
            CanShowEmergencyContacts = accessSetting.ShowEmergencyContacts,
            EmergencyContacts = accessSetting.ShowEmergencyContacts
                ? profile.EmergencyContacts
                    .OrderBy(x => x.Priority)
                    .Select(x => new PublicEmergencyContactResponse
                    {
                        FullName = x.FullName,
                        Relationship = x.Relationship,
                        PhoneNumber = x.PhoneNumber,
                        Priority = x.Priority,
                        IsPrimary = x.IsPrimary
                    })
                    .ToList()
                : [],
            MedicalInfo = accessSetting.ShowMedicalInfo && profile.MedicalInfo is not null
                ? new PublicMedicalInfoResponse
                {
                    BloodType = profile.MedicalInfo.BloodType,
                    ChronicDiseases = profile.MedicalInfo.ChronicDiseases,
                    Allergies = profile.MedicalInfo.Allergies,
                    CurrentMedications = profile.MedicalInfo.CurrentMedications,
                    EmergencyInstructions = profile.MedicalInfo.EmergencyInstructions
                }
                : null
        };
    }

    private static ScanLogResponse MapToScanLogResponse(ScanLog scanLog)
    {
        return new ScanLogResponse
        {
            Id = scanLog.Id,
            ProfileId = scanLog.ProfileId,
            ScannedAtUtc = scanLog.ScannedAtUtc,
            IpAddress = scanLog.IpAddress,
            UserAgent = scanLog.UserAgent,
            LocationText = scanLog.LocationText,
            Latitude = scanLog.Latitude,
            Longitude = scanLog.Longitude,
            ActionType = scanLog.ActionType,
            Note = scanLog.Note
        };
    }

    private static string? TrimOrNull(string? value, int maxLength)
    {
        if (string.IsNullOrWhiteSpace(value))
        {
            return null;
        }

        var trimmed = value.Trim();
        return trimmed.Length <= maxLength ? trimmed : trimmed[..maxLength];
    }
}
