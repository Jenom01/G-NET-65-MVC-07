using GymManagementSystem_BLL.Results;
using GymManagementSystem_BLL.ViewModels.Common;
using GymManagementSystem_BLL.ViewModels.Sessions;

namespace GymManagementSystem_BLL.Services
{
    public interface ISessionService
    {
        Task<IEnumerable<SessionViewModel>> GetAllAsync(CancellationToken ct = default);
        Task<Result<SessionViewModel>> GetDetailsAsync(int id, CancellationToken ct = default);
        Task<IReadOnlyList<LookupItem>> GetCategoryLookupsAsync(CancellationToken ct = default);
        Task<IReadOnlyList<LookupItem>> GetTrainerLookupsAsync(CancellationToken ct = default);
        Task<Result> CreateAsync(SessionCreateViewModel model, CancellationToken ct = default);
        Task<Result<SessionEditViewModel>> GetForUpdateAsync(int id, CancellationToken ct = default);
        Task<Result> UpdateAsync(int id, SessionEditViewModel model, CancellationToken ct = default);
        Task<Result<SessionViewModel>> GetForDeleteAsync(int id, CancellationToken ct = default);
        Task<Result> DeleteAsync(int id, CancellationToken ct = default);
    }
}
