using GymManagementSystem_DAL.Models;

namespace GymManagementSystem_DAL.Repositories
{
    public interface IUserRepository<TUser> : IGenericRepository<TUser> where TUser : User
    {
        Task<bool> IsEmailTakenAsync(string email, int? excludeId = null, CancellationToken ct = default);
        Task<bool> IsPhoneTakenAsync(string phone, int? excludeId = null, CancellationToken ct = default);
    }
}
