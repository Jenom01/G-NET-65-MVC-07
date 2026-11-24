using GymManagementSystem_BLL.Results;
using GymManagementSystem_BLL.ViewModels.Plans;

namespace GymManagementSystem_BLL.Services
{
    public interface IPlanService
    {
        Task<IEnumerable<PlanViewModel>> GetAllAsync(CancellationToken ct = default);
        Task<Result<PlanViewModel>> GetDetailsAsync(int id, CancellationToken ct = default);
        Task<Result<PlanEditViewModel>> GetForUpdateAsync(int id, CancellationToken ct = default);
        Task<Result> UpdateAsync(int id, PlanEditViewModel model, CancellationToken ct = default);
        Task<Result> ToggleStatusAsync(int id, CancellationToken ct = default);
    }
}
