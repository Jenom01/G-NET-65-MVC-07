using GymManagementSystem_BLL.Results;

namespace GymManagementSystem_BLL.Errors
{
    public static class TrainerErrors
    {
        public static readonly Error NotFound =
            new("Trainer.NotFound", "Trainer not found.", ErrorType.NotFound);

        public static readonly Error EmailExists =
            new("Trainer.EmailExists", "This email is already used", ErrorType.Conflict, "Email");

        public static readonly Error PhoneExists =
            new("Trainer.PhoneExists", "This phone number is already used", ErrorType.Conflict, "Phone");

        public static readonly Error HasActiveSessions =
            new("Trainer.HasActiveSessions", "Cannot delete a trainer who has scheduled sessions", ErrorType.Conflict);
    }
}
