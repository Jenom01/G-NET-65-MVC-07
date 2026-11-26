using GymManagementSystem_DAL.Data.Identity;
using GymManagementSystem_DAL.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;
using System.Reflection;

namespace GymManagementSystem_DAL.Data
{
    public class GymDbContext : IdentityDbContext<GymUser, GymRole, Guid>
    {
        public GymDbContext(DbContextOptions<GymDbContext> options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());

            foreach (var entityType in modelBuilder.Model.GetEntityTypes()
                .Where(t => t.BaseType == null &&
                            typeof(BaseEntity).IsAssignableFrom(t.ClrType)))
            {
                var parameter = Expression.Parameter(entityType.ClrType, "e");
                var isDeleted = Expression.Property(parameter, nameof(BaseEntity.IsDeleted));
                var filter = Expression.Lambda(Expression.Not(isDeleted), parameter);

                entityType.SetQueryFilter(filter);
            }
        }

        public DbSet<Plan> Plans => Set<Plan>();
        public DbSet<Category> Categories => Set<Category>();
        public DbSet<User> GymUsers => Set<User>();
        public DbSet<Member> Members => Set<Member>();
        public DbSet<Trainer> Trainers => Set<Trainer>();
        public DbSet<HealthRecord> HealthRecords => Set<HealthRecord>();
        public DbSet<Session> Sessions => Set<Session>();
        public DbSet<Membership> Memberships => Set<Membership>();
        public DbSet<Booking> Bookings => Set<Booking>();
    }
}
