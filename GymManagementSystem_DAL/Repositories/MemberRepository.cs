using GymManagementSystem_DAL.Data;
using GymManagementSystem_DAL.Models;
using Microsoft.EntityFrameworkCore;

namespace GymManagementSystem_DAL.Repositories
{
    public class MemberRepository(GymDbContext dbContext) : UserRepository<Member>(dbContext),
        IMemberRepository
    {
        public async Task<Member?> GetDetailsAsync(int id, CancellationToken ct = default)
            => await _dbSet.AsNoTracking()
                     .Include(m => m.Memberships)
                         .ThenInclude(ms => ms.Plan)
                     .FirstOrDefaultAsync(m => m.Id == id, ct);

        public async Task<bool> HasActiveBookingsAsync(int id, CancellationToken ct = default)
        {
            var now = DateTime.UtcNow;

            return await _dbSet
                .Where(m => m.Id == id)
                .SelectMany(m => m.Bookings)
                .AnyAsync(b => b.Session.EndDate > now, ct);
        }

        public async Task<string?> GetPhotoKeyAsync(int id, CancellationToken ct = default)
            => await _dbSet.Where(m => m.Id == id).Select(m => m.Photo).FirstOrDefaultAsync(ct);
    }
}
