using GymManagementSystem_DAL.Models;

namespace GymManagementSystem_DAL.Repositories
{
    public interface IMemberRepository : IUserRepository<Member>
    {
        Task<Member?> GetDetailsAsync(int id, CancellationToken ct = default);
        Task<bool> HasActiveBookingsAsync(int id, CancellationToken ct = default);
        Task<string?> GetPhotoKeyAsync(int id, CancellationToken ct = default);
    }
}
