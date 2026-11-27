using GymManagementSystem_DAL.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace GymManagementSystem_DAL.Data.Configurations
{
    public class PersonConfiguration : IEntityTypeConfiguration<Person>
    {
        public virtual void Configure(EntityTypeBuilder<Person> builder)
        {
            builder.ToTable("People");

            builder.Property(u => u.Name)
                .HasMaxLength(50);

            builder.Property(u => u.Email)
                .HasMaxLength(100)
                .IsUnicode(false);

            builder.Property(u => u.Phone)
                .HasMaxLength(11)
                .IsUnicode(false)
                .IsRequired();

            builder.Property(u => u.Gender)
                .HasConversion<string>()
                .HasMaxLength(20);

            builder.HasDiscriminator<string>("PersonType")
                .HasValue<Member>("Member")
                .HasValue<Trainer>("Trainer");

            builder.Property("PersonType")
                .HasMaxLength(20);

            builder.OwnsOne(u => u.Address, t =>
            {
                t.Property(a => a.City)
                    .HasColumnName("City")
                    .HasMaxLength(30);

                t.Property(a => a.Street)
                    .HasColumnName("Street")
                    .HasMaxLength(30);

                t.Property(a => a.BuildingNumber)
                    .HasColumnName("BuildingNumber");
            });

            builder.HasIndex(u => u.Email)
                .IsUnique()
                .HasFilter("[IsDeleted] = 0");

            builder.HasIndex(u => u.Phone)
                .IsUnique()
                .HasFilter("[IsDeleted] = 0");

            builder.ToTable(t =>
            {
                t.HasCheckConstraint(
                    "CK_People_Phone",
                    "LEN([Phone]) = 11 AND [Phone] LIKE '01[0125]%' AND [Phone] NOT LIKE '%[^0-9]%'"
                );

                t.HasCheckConstraint(
                    "CK_People_Email",
                    "[Email] LIKE '_%@_%._%'"
                );

                t.HasCheckConstraint(
                    "CK_People_Trainer_Speciality",
                    "[PersonType] <> 'Trainer' OR [Speciality] IS NOT NULL"
                );
            });
        }
    }
}
