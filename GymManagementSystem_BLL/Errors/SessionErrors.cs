using GymManagementSystem_BLL.Results;

namespace GymManagementSystem_BLL.Errors
{
    public static class SessionErrors
    {
        public static readonly Error NotFound = new("Session.NotFound",
            "Session not found.", ErrorType.NotFound);

        public static readonly Error InvalidDateRange = new("Session.InvalidDateRange",
            "End time must be after start time", ErrorType.Validation, "EndDate");

        public static readonly Error StartInPast = new("Session.StartInPast",
            "Start time must be in the future", ErrorType.Validation, "StartDate");

        public static readonly Error InvalidLocalTime = new("Session.InvalidLocalTime",
            "This time does not exist because of a daylight-saving change. Pick another time", ErrorType.Validation, "StartDate");

        public static readonly Error CategoryNotFound = new("Session.CategoryNotFound",
            "Selected category does not exist", ErrorType.Validation, "CategoryId");

        public static readonly Error TrainerNotFound = new("Session.TrainerNotFound",
            "Selected trainer does not exist", ErrorType.Validation, "TrainerId");

        public static readonly Error SpecialityMismatch = new("Session.SpecialityMismatch",
            "The trainer's specialty does not match the selected category", ErrorType.Validation, "TrainerId");

        public static readonly Error TrainerBusy = new("Session.TrainerBusy",
            "This trainer already has a session in that time slot", ErrorType.Conflict, "TrainerId");

        public static readonly Error NotEditable = new("Session.NotEditable",
            "Only upcoming sessions can be edited", ErrorType.Conflict);

        public static readonly Error CannotDeleteOngoing = new("Session.CannotDeleteOngoing",
            "An ongoing session can't be deleted because members are training right now", ErrorType.Conflict);
    }
}
