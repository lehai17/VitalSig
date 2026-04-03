using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Vitalsig.API.Domain.Entities;

namespace Vitalsig.API.Infrastructure.Data.Configurations;

public class ProfileConfiguration : IEntityTypeConfiguration<Profile>
{
    public void Configure(EntityTypeBuilder<Profile> builder)
    {
        builder.ToTable("Profiles");

        builder.HasKey(x => x.Id);

        builder.Property(x => x.ProfileCode).HasMaxLength(50).IsRequired();
        builder.Property(x => x.PublicToken).HasMaxLength(100).IsRequired();
        builder.Property(x => x.DisplayName).HasMaxLength(150).IsRequired();
        builder.Property(x => x.AvatarUrl).HasMaxLength(500);
        builder.Property(x => x.IdentificationNote).HasMaxLength(1000);
        builder.Property(x => x.AddressNote).HasMaxLength(500);

        builder.HasIndex(x => x.ProfileCode).IsUnique();
        builder.HasIndex(x => x.PublicToken).IsUnique();
        builder.HasIndex(x => x.OwnerUserId);

        builder.HasOne(x => x.OwnerUser)
            .WithMany(x => x.Profiles)
            .HasForeignKey(x => x.OwnerUserId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasOne(x => x.MedicalInfo)
            .WithOne(x => x.Profile)
            .HasForeignKey<MedicalInfo>(x => x.ProfileId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasOne(x => x.AccessSetting)
            .WithOne(x => x.Profile)
            .HasForeignKey<ProfileAccessSetting>(x => x.ProfileId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}
