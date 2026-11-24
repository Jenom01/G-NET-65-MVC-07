namespace GymManagementSystem_BLL.ViewModels.Trainers
{
    public class TrainerDetailsViewModel
    {
        public int Id { get; set; }
        public string Name { get; set; } = default!;
        public string Speciality { get; set; } = default!;
        public string Email { get; set; } = default!;
        public string Phone { get; set; } = default!;
        public DateOnly DateOfBirth { get; set; }
        public string Address { get; set; } = default!;
    }
}
