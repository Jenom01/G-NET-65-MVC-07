using Microsoft.AspNetCore.Identity;

namespace GymManagementSystem_DAL.Data.Identity
{
    public class GymRole : IdentityRole<Guid>
    {
        public string DisplayName { get; set; } = default!;
    }
}
