using GymManagementSystem_DAL.Models;

namespace GymManagementSystem_DAL.Repositories
{
    public interface IPlanRepository : IGenericRepository<Plan>
    {
        Task<bool> HasActiveMembershipsAsync(int id, CancellationToken ct = default);
    }
}
