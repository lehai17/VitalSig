using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Vitalsig.API.Domain.Entities;

namespace Vitalsig.API.Infrastructure.Data.Configurations;

public class ProfileAccessSettingConfiguration : IEntityTypeConfiguration<ProfileAccessSetting>
{
    public void Configure(EntityTypeBuilder<ProfileAccessSetting> builder)
    {
        builder.ToTable("ProfileAccessSettings");

        builder.HasKey(x => x.Id);

        builder.HasIndex(x => x.ProfileId).IsUnique();
    }
}
