namespace GymManagementSystem_BLL.ViewModels.Analytics
{
    public class AnalyticsViewModel
    {
        public int TotalMembers { get; init; }
        public int ActiveMembers { get; init; }
        public int TotalTrainers { get; init; }
        public int UpcomingSessions { get; init; }
        public int OngoingSessions { get; init; }
        public int CompletedSessions { get; init; }
    }
}
