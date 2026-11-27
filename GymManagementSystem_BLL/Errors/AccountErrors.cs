using GymManagementSystem_BLL.Results;

namespace GymManagementSystem_BLL.Errors
{
    public static class AccountErrors
    {
        public static readonly Error InvalidCredentials =
            new("Account.InvalidCredentials", "Invalid email or password.", ErrorType.Validation);

        public static readonly Error LockedOut =
            new("Account.LockedOut", "Too many failed attempts. Try again later.", ErrorType.Failure);
    }
}
