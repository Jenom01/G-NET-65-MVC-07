using GymManagementSystem_DAL.Data;
using GymManagementSystem_DAL.Models;
using Microsoft.EntityFrameworkCore;

namespace GymManagementSystem_DAL.Repositories
{
    public class SessionRepository(GymDbContext dbContext)
        : GenericRepository<Session>(dbContext), ISessionRepository
    {
        public async Task<IReadOnlyList<Session>> GetAllWithDetailsAsync(CancellationToken ct = default)
            => await WithDetails().OrderBy(s => s.StartDate).ToListAsync(ct);

        public async Task<Session?> GetDetailsAsync(int id, CancellationToken ct = default)
            => await WithDetails().FirstOrDefaultAsync(s => s.Id == id, ct);

        private IQueryable<Session> WithDetails()
            => _dbSet.AsNoTracking()
                .Include(s => s.Trainer)
                .Include(s => s.Category)
                .Include(s => s.Bookings);

        public async Task<bool> HasTrainerConflictAsync(int trainerId, DateTime startUtc, DateTime endUtc, int? excludeSessionId = null, CancellationToken ct = default)
            => await _dbSet.AnyAsync(s =>
                s.TrainerId == trainerId
                && (excludeSessionId == null || s.Id != excludeSessionId)
                && s.StartDate < endUtc
                && s.EndDate > startUtc, ct);
    }
}
