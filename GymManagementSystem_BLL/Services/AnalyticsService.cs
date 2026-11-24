using GymManagementSystem_BLL.ViewModels.Analytics;
using GymManagementSystem_DAL.Repositories;

namespace GymManagementSystem_BLL.Services
{
    internal class AnalyticsService(IUnitOfWork uow, TimeProvider clock)
        : IAnalyticsService
    {
        public async Task<AnalyticsViewModel> GetDashboardAsync(CancellationToken ct = default)
        {
            var now = clock.GetUtcNow().UtcDateTime;

            return new AnalyticsViewModel
            {
                TotalMembers = await uow.Members.CountAsync(ct: ct),

                ActiveMembers = await uow.Members.CountAsync(
                    m => m.Memberships.Any(ms => ms.StartDate <= now && ms.EndDate > now), ct),

                TotalTrainers = await uow.Trainers.CountAsync(ct: ct),

                UpcomingSessions = await uow.Sessions.CountAsync(s => s.StartDate > now, ct),
                OngoingSessions = await uow.Sessions.CountAsync(s => s.StartDate <= now && s.EndDate > now, ct),
                CompletedSessions = await uow.Sessions.CountAsync(s => s.EndDate <= now, ct)
            };
        }
    }
}
