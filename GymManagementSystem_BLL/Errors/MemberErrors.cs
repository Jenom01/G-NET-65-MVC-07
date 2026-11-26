using GymManagementSystem_BLL.Results;

namespace GymManagementSystem_BLL.Errors
{
    public static class MemberErrors
    {
        public static readonly Error NotFound =
            new("Member.NotFound", "Member not found.", ErrorType.NotFound);

        public static readonly Error HealthRecordNotFound =
            new("Member.HealthRecordNotFound", "Health record not found.", ErrorType.NotFound);

        public static readonly Error EmailExists =
            new("Member.EmailExists", "This email is already used", ErrorType.Conflict, "Email");

        public static readonly Error PhoneExists =
            new("Member.PhoneExists", "This phone number is already used", ErrorType.Conflict, "Phone");

        public static readonly Error PhotoNotFound =
            new("Member.PhotoNotFound", "Member photo not found.", ErrorType.NotFound);

        public static readonly Error HasActiveBookings =
            new("Member.HasActiveBookings", "Cannot delete a member with active bookings", ErrorType.Conflict);
    }
}
