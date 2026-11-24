using GymManagementSystem_DAL.Data;
using GymManagementSystem_DAL.Models;
using Microsoft.EntityFrameworkCore;

namespace GymManagementSystem_DAL.Repositories
{
    public class TrainerRepository(GymDbContext dbContext) : UserRepository<Trainer>(dbContext),
        ITrainerRepository
    {
        public async Task<bool> HasActiveSessionsAsync(int id, CancellationToken ct = default)
        {
            var now = DateTime.UtcNow;

            return await _dbSet
                .Where(t => t.Id == id)
                .SelectMany(t => t.Sessions)
                .AnyAsync(s => s.EndDate > now, ct);
        }
    }
}
