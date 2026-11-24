using GymManagementSystem_DAL.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace GymManagementSystem_DAL.Data.Configurations
{
    public class MembershipConfiguration : IEntityTypeConfiguration<Membership>
    {
        public void Configure(EntityTypeBuilder<Membership> builder)
        {
            builder.Property(m => m.StartDate)
                .HasDefaultValueSql("GETUTCDATE()");

            builder.ToTable(t =>
            {
                t.HasCheckConstraint(
                    "CK_Membership_DateRange",
                    "[EndDate] > [StartDate]"
                );
            });

            builder.HasIndex(m => new { m.MemberId, m.PlanId, m.StartDate })
                .IsUnique()
                .HasFilter("[IsDeleted] = 0");
        }
    }
}
