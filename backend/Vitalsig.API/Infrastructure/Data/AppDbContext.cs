using Microsoft.EntityFrameworkCore;
using Vitalsig.API.Domain.Entities;

namespace Vitalsig.API.Infrastructure.Data;

public class AppDbContext(DbContextOptions<AppDbContext> options) : DbContext(options)
{
    public DbSet<User> Users => Set<User>();
    public DbSet<Profile> Profiles => Set<Profile>();
    public DbSet<EmergencyContact> EmergencyContacts => Set<EmergencyContact>();
    public DbSet<MedicalInfo> MedicalInfos => Set<MedicalInfo>();
    public DbSet<ProfileAccessSetting> ProfileAccessSettings => Set<ProfileAccessSetting>();
    public DbSet<QrCodeRecord> QrCodes => Set<QrCodeRecord>();
    public DbSet<ScanLog> ScanLogs => Set<ScanLog>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.ApplyConfigurationsFromAssembly(typeof(AppDbContext).Assembly);
    }
}
