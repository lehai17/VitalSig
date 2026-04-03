using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Vitalsig.API.Domain.Entities;

namespace Vitalsig.API.Infrastructure.Data.Configurations;

public class QrCodeRecordConfiguration : IEntityTypeConfiguration<QrCodeRecord>
{
    public void Configure(EntityTypeBuilder<QrCodeRecord> builder)
    {
        builder.ToTable("QrCodes");

        builder.HasKey(x => x.Id);

        builder.Property(x => x.QrValue).HasMaxLength(1000).IsRequired();
        builder.Property(x => x.QrImageUrl).HasMaxLength(500);

        builder.HasIndex(x => x.ProfileId);

        builder.HasOne(x => x.Profile)
            .WithMany(x => x.QrCodes)
            .HasForeignKey(x => x.ProfileId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}
