using GymManagementSystem_DAL.Models;

namespace GymManagementSystem_DAL.Repositories
{
    public interface ITrainerRepository : IUserRepository<Trainer>
    {
        Task<bool> HasActiveSessionsAsync(int id, CancellationToken ct = default);
    }
}
