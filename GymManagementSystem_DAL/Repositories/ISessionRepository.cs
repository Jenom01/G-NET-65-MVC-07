using GymManagementSystem_DAL.Models;

namespace GymManagementSystem_DAL.Repositories
{
    public interface ISessionRepository : IGenericRepository<Session>
    {
        Task<IReadOnlyList<Session>> GetAllWithDetailsAsync(CancellationToken ct = default);
        Task<Session?> GetDetailsAsync(int id, CancellationToken ct = default);
        Task<bool> HasTrainerConflictAsync(int trainerId, DateTime startUtc, DateTime endUtc,
            int? excludeSessionId = null, CancellationToken ct = default);
    }
}
