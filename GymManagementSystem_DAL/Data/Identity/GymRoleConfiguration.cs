using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace GymManagementSystem_DAL.Data.Identity
{
    public class GymRoleConfiguration : IEntityTypeConfiguration<GymRole>
    {
        public void Configure(EntityTypeBuilder<GymRole> builder)
        {
            builder.Property(role => role.DisplayName)
                .HasMaxLength(100)
                .IsRequired();
        }
    }
}
