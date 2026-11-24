namespace GymManagementSystem_BLL.ViewModels.Members
{
    public class MemberIndexViewModel
    {
        public int Id { get; set; }
        public string Name { get; set; } = default!;
        public string Email { get; set; } = default!;
        public string Phone { get; set; } = default!;
        public string? PhotoUrl { get; set; }
        public DateTime JoinDate { get; set; }
        public string Gender { get; set; } = default!;
    }
}
