using GymManagementSystem_DAL.Data.Identity;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;

namespace GymManagementSystem_DAL.Data.Seed
{
    public static class IdentitySeeder
    {
        public static async Task SeedAsync(UserManager<GymUser> userManager,
            RoleManager<GymRole> roleManager, IConfiguration config)
        {
            if (!await roleManager.RoleExistsAsync(IdentityRoleNames.SuperAdmin))
            {
                var result = await roleManager.CreateAsync(new GymRole
                {
                    Name = IdentityRoleNames.SuperAdmin,
                    DisplayName = "Super Admin"
                });

                if (!result.Succeeded)
                    throw new InvalidOperationException("Failed to create role");
            }

            if (!await roleManager.RoleExistsAsync(IdentityRoleNames.Admin))
            {
                var result = await roleManager.CreateAsync(new GymRole
                {
                    Name = IdentityRoleNames.Admin,
                    DisplayName = "Admin"
                });

                if (!result.Succeeded)
                    throw new InvalidOperationException("Failed to create role");
            }

            var superAdminEmail = config["IdentitySeed:SuperAdminEmail"] ?? throw new InvalidOperationException();
            var superAdminPassword = config["IdentitySeed:SuperAdminPassword"] ?? throw new InvalidOperationException();

            var AdminEmail = config["IdentitySeed:AdminEmail"] ?? throw new InvalidOperationException();
            var AdminPassword = config["IdentitySeed:AdminPassword"] ?? throw new InvalidOperationException();

            var superAdmin = await userManager.FindByEmailAsync(superAdminEmail);

            if (superAdmin is null)
            {
                superAdmin = new GymUser
                {
                    UserName = superAdminEmail,
                    Email = superAdminEmail,
                    FullName = "Super Administrator",
                    EmailConfirmed = true
                };

                var result = await userManager.CreateAsync(superAdmin, superAdminPassword);

                if (!result.Succeeded)
                    throw new InvalidOperationException("Failed to create user");
            }

            var Admin = await userManager.FindByEmailAsync(AdminEmail);

            if (Admin is null)
            {
                Admin = new GymUser
                {
                    UserName = AdminEmail,
                    Email = AdminEmail,
                    FullName = "Administrator",
                    EmailConfirmed = true
                };

                var result = await userManager.CreateAsync(Admin, AdminPassword);

                if (!result.Succeeded)
                    throw new InvalidOperationException("Failed to create user");
            }

            if (!await userManager.IsInRoleAsync(superAdmin, IdentityRoleNames.SuperAdmin))
            {
                var result = await userManager.AddToRoleAsync(superAdmin, IdentityRoleNames.SuperAdmin);

                if (!result.Succeeded)
                    throw new InvalidOperationException("Failed to add user to role");
            }

            if (!await userManager.IsInRoleAsync(Admin, IdentityRoleNames.Admin))
            {
                var result = await userManager.AddToRoleAsync(Admin, IdentityRoleNames.Admin);

                if (!result.Succeeded)
                    throw new InvalidOperationException("Failed to add user to role");
            }
        }
    }
}
