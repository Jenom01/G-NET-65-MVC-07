using System.ComponentModel.DataAnnotations;

namespace GymManagementSystem_DAL.Data.Seed
{
    public sealed class IdentitySeedOptions
    {
        public const string SectionName = "IdentitySeed";

        [Required, EmailAddress]
        public string SuperAdminEmail { get; set; } = default!;

        [Required]
        public string SuperAdminPassword { get; set; } = default!;

        [Required, EmailAddress]
        public string AdminEmail { get; set; } = default!;

        [Required]
        public string AdminPassword { get; set; } = default!;
    }
}
