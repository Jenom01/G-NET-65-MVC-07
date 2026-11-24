using GymManagementSystem_BLL.ViewModels.Analytics;

namespace GymManagementSystem_BLL.Services
{
    public interface IAnalyticsService
    {
        Task<AnalyticsViewModel> GetDashboardAsync(CancellationToken ct = default);
    }
}
