using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Vitalsig.API.Domain.Entities;

namespace Vitalsig.API.Infrastructure.Data.Configurations;

public class MedicalInfoConfiguration : IEntityTypeConfiguration<MedicalInfo>
{
    public void Configure(EntityTypeBuilder<MedicalInfo> builder)
    {
        builder.ToTable("MedicalInfos");

        builder.HasKey(x => x.Id);

        builder.Property(x => x.BloodType).HasMaxLength(10);
        builder.Property(x => x.ChronicDiseases).HasMaxLength(2000);
        builder.Property(x => x.Allergies).HasMaxLength(2000);
        builder.Property(x => x.CurrentMedications).HasMaxLength(2000);
        builder.Property(x => x.EmergencyInstructions).HasMaxLength(2000);
        builder.Property(x => x.DoctorName).HasMaxLength(150);
        builder.Property(x => x.DoctorPhone).HasMaxLength(20);

        builder.HasIndex(x => x.ProfileId).IsUnique();
    }
}
