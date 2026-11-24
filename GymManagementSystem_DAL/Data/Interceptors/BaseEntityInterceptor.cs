using GymManagementSystem_DAL.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;

namespace GymManagementSystem_DAL.Data.Interceptors
{
    public class BaseEntityInterceptor : SaveChangesInterceptor
    {
        public override InterceptionResult<int> SavingChanges(DbContextEventData eventData, InterceptionResult<int> result)
        {
            Apply(eventData.Context);
            return base.SavingChanges(eventData, result);
        }

        public override ValueTask<InterceptionResult<int>> SavingChangesAsync(DbContextEventData eventData, InterceptionResult<int> result, CancellationToken cancellationToken = default)
        {
            Apply(eventData.Context);
            return base.SavingChangesAsync(eventData, result, cancellationToken);
        }

        private static void Apply(DbContext? context)
        {
            if (context is null) return;

            var now = DateTime.UtcNow;

            foreach (var entry in context.ChangeTracker.Entries<BaseEntity>().ToList())
            {
                switch (entry.State)
                {
                    case EntityState.Added:
                        entry.Entity.CreatedAt = now;
                        if (entry.Entity is Member member) member.JoinDate ??= now;
                        if (entry.Entity is Trainer trainer) trainer.HireDate ??= now;
                        break;

                    case EntityState.Modified:
                        entry.Property(nameof(BaseEntity.CreatedAt)).IsModified = false;
                        entry.Entity.LastUpdatedAt = now;
                        break;

                    case EntityState.Deleted:
                        entry.State = EntityState.Modified;
                        entry.Entity.IsDeleted = true;
                        entry.Entity.DeletedAt = now;

                        foreach (var reference in entry.References)
                            if (reference.TargetEntry is { } target && target.Metadata.IsOwned())
                                target.State = EntityState.Unchanged;
                        break;

                    case EntityState.Unchanged when entry.References.Any(r => r.TargetEntry is { } t && t.Metadata.IsOwned() && t.State == EntityState.Modified):
                        entry.Property(nameof(BaseEntity.LastUpdatedAt)).CurrentValue = now;
                        break;
                }
            }
        }
    }
}
