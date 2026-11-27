using GymManagementSystem_DAL.Data.Identity;
using Microsoft.AspNetCore.Identity;

namespace GymManagementSystem_DAL.Data.Seed
{
    public static class IdentitySeeder
    {
        public static async Task SeedAsync(UserManager<GymUser> userManager,
            RoleManager<GymRole> roleManager, IdentitySeedOptions options)
        {
            ArgumentNullException.ThrowIfNull(userManager);
            ArgumentNullException.ThrowIfNull(roleManager);
            ArgumentNullException.ThrowIfNull(options);

            await EnsureRoleAsync(roleManager, RoleNames.SuperAdmin, "Super Admin");
            await EnsureRoleAsync(roleManager, RoleNames.Admin, "Admin");

            var superAdmin = await EnsureUserAsync(userManager,
                options.SuperAdminEmail, options.SuperAdminPassword, "Super Administrator");
            var admin = await EnsureUserAsync(userManager,
                options.AdminEmail, options.AdminPassword, "Administrator");

            await EnsureUserInRoleAsync(userManager, superAdmin, RoleNames.SuperAdmin);
            await EnsureUserInRoleAsync(userManager, admin, RoleNames.Admin);
        }

        private static async Task EnsureRoleAsync(RoleManager<GymRole> roleManager,
            string roleName, string displayName)
        {
            if (await roleManager.RoleExistsAsync(roleName)) return;

            var role = new GymRole
            {
                Name = roleName,
                DisplayName = displayName
            };

            var result = await roleManager.CreateAsync(role);
            EnsureSucceeded(result, $"create role '{roleName}'");
        }

        private static async Task<GymUser> EnsureUserAsync(UserManager<GymUser> userManager,
            string email, string password, string fullName)
        {
            var user = await userManager.FindByEmailAsync(email);

            if (user is not null) return user;

            user = new GymUser
            {
                UserName = email,
                Email = email,
                FullName = fullName,
                EmailConfirmed = true
            };

            var result = await userManager.CreateAsync(user, password);
            EnsureSucceeded(result, $"create user '{email}'");

            return user;
        }

        private static async Task EnsureUserInRoleAsync(UserManager<GymUser> userManager,
            GymUser user, string roleName)
        {
            if (await userManager.IsInRoleAsync(user, roleName)) return;

            var result = await userManager.AddToRoleAsync(user, roleName);
            EnsureSucceeded(result, $"assign role '{roleName}' to user '{user.Email}'");
        }

        private static void EnsureSucceeded(IdentityResult result, string operation)
        {
            if (result.Succeeded) return;

            var errors = string.Join("; ",
                result.Errors.Select(error => $"{error.Code}: {error.Description}"));

            throw new InvalidOperationException($"Could not {operation}. Identity errors: {errors}");
        }
    }
}