using GymManagementSystem_DAL.Enums;
using GymManagementSystem_DAL.Models.ValueObjects;

namespace GymManagementSystem_DAL.Models
{
    public abstract class User : BaseEntity
    {
        public string Name { get; set; } = default!;
        public string Email { get; set; } = default!;
        public string Phone { get; set; } = default!;

        public DateOnly DateOfBirth { get; set; }
        public Gender Gender { get; set; }
        public Address Address { get; set; } = default!;
    }
}
