using Microsoft.AspNetCore.Identity;

namespace GymManagementSystem_DAL.Data.Identity
{
    public class GymUser : IdentityUser<Guid>
    {
        public string FullName { get; set; } = default!;
    }
}
