using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Vitalsig.API.Domain.Entities;

namespace Vitalsig.API.Infrastructure.Data.Configurations;

public class ScanLogConfiguration : IEntityTypeConfiguration<ScanLog>
{
    public void Configure(EntityTypeBuilder<ScanLog> builder)
    {
        builder.ToTable("ScanLogs");

        builder.HasKey(x => x.Id);

        builder.Property(x => x.IpAddress).HasMaxLength(45);
        builder.Property(x => x.UserAgent).HasMaxLength(1000);
        builder.Property(x => x.LocationText).HasMaxLength(255);
        builder.Property(x => x.ActionType).HasMaxLength(100);
        builder.Property(x => x.Note).HasMaxLength(1000);
        builder.Property(x => x.Latitude).HasColumnType("decimal(9,6)");
        builder.Property(x => x.Longitude).HasColumnType("decimal(9,6)");

        builder.HasIndex(x => new { x.ProfileId, x.ScannedAtUtc });

        builder.HasOne(x => x.Profile)
            .WithMany(x => x.ScanLogs)
            .HasForeignKey(x => x.ProfileId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}
