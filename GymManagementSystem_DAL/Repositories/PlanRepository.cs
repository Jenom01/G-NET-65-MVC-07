using GymManagementSystem_DAL.Data;
using GymManagementSystem_DAL.Models;
using Microsoft.EntityFrameworkCore;

namespace GymManagementSystem_DAL.Repositories
{
    public class PlanRepository(GymDbContext dbContext)
        : GenericRepository<Plan>(dbContext), IPlanRepository
    {
        public async Task<bool> HasActiveMembershipsAsync(int id, CancellationToken ct = default)
        {
            var now = DateTime.UtcNow;

            return await _dbSet
                .Where(p => p.Id == id)
                .SelectMany(p => p.Memberships)
                .AnyAsync(m => m.EndDate > now && !m.Member.IsDeleted, ct);
        }
    }
}
