using GymManagementSystem_BLL.Results;
using GymManagementSystem_BLL.ViewModels.Trainers;

namespace GymManagementSystem_BLL.Services
{
    public interface ITrainerService
    {
        Task<IEnumerable<TrainerViewModel>> GetAllAsync(CancellationToken ct = default);
        Task<Result> CreateAsync(TrainerCreateViewModel model, CancellationToken ct = default);
        Task<Result<TrainerDetailsViewModel>> GetDetailsAsync(int id, CancellationToken ct = default);
        Task<Result<TrainerEditViewModel>> GetForUpdateAsync(int id, CancellationToken ct = default);
        Task<Result> UpdateAsync(int id, TrainerEditViewModel model, CancellationToken ct = default);
        Task<Result> DeleteAsync(int id, CancellationToken ct = default);
    }
}
