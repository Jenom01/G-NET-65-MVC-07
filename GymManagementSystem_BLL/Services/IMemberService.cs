using GymManagementSystem_BLL.Results;
using GymManagementSystem_BLL.ViewModels.HealthRecords;
using GymManagementSystem_BLL.ViewModels.Members;

namespace GymManagementSystem_BLL.Services
{
    public interface IMemberService
    {
        Task<IEnumerable<MemberIndexViewModel>> GetAllAsync(CancellationToken ct = default);
        Task<Result> CreateAsync(MemberCreateViewModel model, CancellationToken ct = default);
        Task<Result<MemberDetailsViewModel>> GetDetailsAsync(int id, CancellationToken ct = default);
        Task<Result<HealthRecordDetailsViewModel>> GetHealthRecordAsync(int memberId, CancellationToken ct = default);
        Task<Result<MemberEditViewModel>> GetForUpdateAsync(int id, CancellationToken ct = default);
        Task<Result> UpdateAsync(int id, MemberEditViewModel model, CancellationToken ct = default);
        Task<Result> DeleteAsync(int id, CancellationToken ct = default);
    }
}
