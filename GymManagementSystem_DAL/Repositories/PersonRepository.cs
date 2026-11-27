using GymManagementSystem_DAL.Data;
using GymManagementSystem_DAL.Models;
using Microsoft.EntityFrameworkCore;

namespace GymManagementSystem_DAL.Repositories
{
    public class PersonRepository<TPerson>(GymDbContext dbContext)
        : GenericRepository<TPerson>(dbContext), IPersonRepository<TPerson> where TPerson : Person
    {
        public async Task<bool> IsEmailTakenAsync(string email, int? excludeId = null, CancellationToken ct = default)
            => await _dbContext.Set<Person>().AnyAsync(m => m.Email == email && (!excludeId.HasValue || m.Id != excludeId.Value), ct);

        public async Task<bool> IsPhoneTakenAsync(string phone, int? excludeId = null, CancellationToken ct = default)
            => await _dbContext.Set<Person>().AnyAsync(m => m.Phone == phone && (!excludeId.HasValue || m.Id != excludeId.Value), ct);
    }
}
