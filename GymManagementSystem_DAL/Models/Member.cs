namespace GymManagementSystem_DAL.Models
{
    public class Member : Person
    {
        public string? Photo { get; set; }
        public DateTime? JoinDate { get; set; }

        public HealthRecord? HealthRecord { get; set; }

        public ICollection<Membership> Memberships { get; set; } = [];

        public ICollection<Booking> Bookings { get; set; } = [];
    }
}
