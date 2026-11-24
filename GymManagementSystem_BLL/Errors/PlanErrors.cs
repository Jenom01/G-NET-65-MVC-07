using GymManagementSystem_BLL.Results;

namespace GymManagementSystem_BLL.Errors
{
    public static class PlanErrors
    {
        public static readonly Error NotFound =
            new("Plan.NotFound", "Plan not found.", ErrorType.NotFound);

        public static readonly Error CannotUpdateWithActiveMemberships =
            new("Plan.UpdateBlocked", "Cannot update a plan that has active memberships", ErrorType.Conflict);

        public static readonly Error CannotDeactivateWithActiveMemberships =
            new("Plan.DeactivateBlocked", "Cannot deactivate a plan that has active memberships", ErrorType.Conflict);
    }
}
