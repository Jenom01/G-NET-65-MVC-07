using GymManagementSystem_DAL.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace GymManagementSystem_DAL.Data.Configurations
{
    public class SessionConfiguration : IEntityTypeConfiguration<Session>
    {
        public void Configure(EntityTypeBuilder<Session> builder)
        {
            builder.Property(s => s.Description)
                .HasMaxLength(1000);

            builder.HasMany(s => s.Bookings)
                .WithOne(b => b.Session)
                .HasForeignKey(b => b.SessionId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.ToTable(t =>
            {
                t.HasCheckConstraint(
                    "CK_Sessions_Capacity",
                    "[Capacity] BETWEEN 1 AND 25"
                );

                t.HasCheckConstraint(
                    "CK_Sessions_DateRange",
                    "[EndDate] > [StartDate]"
                );
            });

            builder.HasIndex(s => s.StartDate);
        }
    }
}
