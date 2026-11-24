using GymManagementSystem_DAL.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace GymManagementSystem_DAL.Data.Configurations
{
    public class HealthRecordConfiguration : IEntityTypeConfiguration<HealthRecord>
    {
        public void Configure(EntityTypeBuilder<HealthRecord> builder)
        {
            builder.Property(hr => hr.Height)
                .HasPrecision(5, 2);

            builder.Property(hr => hr.Weight)
                .HasPrecision(5, 2);

            builder.Property(hr => hr.Note)
                .HasMaxLength(500);

            builder.Property(hr => hr.BloodType)
                .HasConversion<string>()
                .HasMaxLength(20);

            builder.HasOne(hr => hr.Member)
                .WithOne(m => m.HealthRecord)
                .HasForeignKey<HealthRecord>(hr => hr.MemberId);

            builder.ToTable(t =>
            {
                t.HasCheckConstraint(
                    "CK_HealthRecord_Height",
                    "[Height] > 0"
                    );

                t.HasCheckConstraint(
                    "CK_HealthRecord_Weight",
                    "[Weight] > 0"
                    );
            });
        }
    }
}
