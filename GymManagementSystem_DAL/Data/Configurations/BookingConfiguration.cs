using GymManagementSystem_DAL.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace GymManagementSystem_DAL.Data.Configurations
{
    public class BookingConfiguration : IEntityTypeConfiguration<Booking>
    {
        public void Configure(EntityTypeBuilder<Booking> builder)
        {
            builder.Property(b => b.BookingDate)
                .HasDefaultValueSql("GETUTCDATE()");

            builder.HasIndex(b => new { b.MemberId, b.SessionId })
                .IsUnique()
                .HasFilter("[IsDeleted] = 0");
        }
    }
}
