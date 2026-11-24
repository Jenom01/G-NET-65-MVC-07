using GymManagementSystem_DAL.Data;
using GymManagementSystem_DAL.Models;
using Microsoft.EntityFrameworkCore;

namespace GymManagementSystem_DAL.Repositories
{
    public class UserRepository<TUser>(GymDbContext dbContext)
        : GenericRepository<TUser>(dbContext), IUserRepository<TUser> where TUser : User
    {
        public async Task<bool> IsEmailTakenAsync(string email, int? excludeId = null, CancellationToken ct = default)
            => await _dbContext.Set<User>().AnyAsync(m => m.Email == email && (!excludeId.HasValue || m.Id != excludeId.Value), ct);

        public async Task<bool> IsPhoneTakenAsync(string phone, int? excludeId = null, CancellationToken ct = default)
            => await _dbContext.Set<User>().AnyAsync(m => m.Phone == phone && (!excludeId.HasValue || m.Id != excludeId.Value), ct);
    }
}
