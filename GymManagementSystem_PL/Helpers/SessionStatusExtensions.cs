using GymManagementSystem_DAL.Enums;

namespace GymManagementSystem_PL.Helpers
{
    public static class SessionStatusExtensions
    {
        public static string ToHeaderClass(this SessionStatus status) => status switch
        {
            SessionStatus.Ongoing => "bg-success",
            SessionStatus.Upcoming => "bg-primary",
            _ => "bg-secondary"
        };
    }
}
