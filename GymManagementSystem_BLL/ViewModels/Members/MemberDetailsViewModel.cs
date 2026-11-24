namespace GymManagementSystem_BLL.ViewModels.Members
{
    public class MemberDetailsViewModel
    {
        public int Id { get; set; }
        public string Name { get; set; } = default!;
        public string? PhotoUrl { get; set; }
        public string Email { get; set; } = default!;
        public string Phone { get; set; } = default!;
        public string Gender { get; set; } = default!;
        public string DateOfBirth { get; set; } = default!;
        public string Address { get; set; } = default!;
        public string PlanName { get; set; } = default!;
        public string MembershipStartDate { get; set; } = default!;
        public string MembershipEndDate { get; set; } = default!;
    }
}
