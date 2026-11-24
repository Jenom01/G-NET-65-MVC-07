using GymManagementSystem_DAL.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace GymManagementSystem_DAL.Data.Configurations
{
    public class PlanConfiguration : IEntityTypeConfiguration<Plan>
    {
        public void Configure(EntityTypeBuilder<Plan> builder)
        {
            builder.Property(p => p.Name)
                .HasMaxLength(50);

            builder.Property(p => p.Description)
                .HasMaxLength(200);

            builder.Property(p => p.Price)
                .HasPrecision(10, 2);

            builder.HasMany(p => p.Memberships)
                .WithOne(m => m.Plan)
                .HasForeignKey(m => m.PlanId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.ToTable(t =>
            {
                t.HasCheckConstraint("CK_Plan_Price",
                    "[Price] >= 0");

                t.HasCheckConstraint("CK_Plan_Duration",
                    "[DurationDays] Between 1 and 365");
            });

            builder.HasIndex(p => p.Name)
                .IsUnique()
                .HasFilter("[IsDeleted] = 0");
        }
    }
}
