using GymManagementSystem_DAL.Enums;

namespace GymManagementSystem_DAL.Models
{
    public class Trainer : User
    {
        public Speciality Speciality { get; set; }
        public DateTime? HireDate { get; set; }

        public ICollection<Session> Sessions { get; set; } = [];
    }
}
