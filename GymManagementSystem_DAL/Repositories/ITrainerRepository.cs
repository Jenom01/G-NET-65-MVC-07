using GymManagementSystem_DAL.Models;

namespace GymManagementSystem_DAL.Repositories
{
    public interface ITrainerRepository : IPersonRepository<Trainer>
    {
        Task<bool> HasActiveSessionsAsync(int id, CancellationToken ct = default);
    }
}
