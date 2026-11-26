using GymManagementSystem_DAL.Data.Identity;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;

namespace GymManagementSystem_DAL.Data.Seed
{
    public static class IdentitySeeder
    {
        public static async Task SeedAsync(UserManager<GymUser> userManager,
            RoleManager<GymRole> roleManager, IConfiguration configuration)
        {
            ArgumentNullException.ThrowIfNull(userManager);
            ArgumentNullException.ThrowIfNull(roleManager);
            ArgumentNullException.ThrowIfNull(configuration);

            await EnsureRoleAsync(roleManager, IdentityRoleNames.SuperAdmin, "Super Admin");
            await EnsureRoleAsync(roleManager, IdentityRoleNames.Admin, "Admin");

            var superAdminEmail = GetRequiredSetting(configuration, "IdentitySeed:SuperAdminEmail");
            var superAdminPassword = GetRequiredSetting(configuration, "IdentitySeed:SuperAdminPassword");

            var adminEmail = GetRequiredSetting(configuration, "IdentitySeed:AdminEmail");
            var adminPassword = GetRequiredSetting(configuration, "IdentitySeed:AdminPassword");

            var superAdmin = await EnsureUserAsync(userManager, superAdminEmail, superAdminPassword, "Super Administrator");
            var admin = await EnsureUserAsync(userManager, adminEmail, adminPassword, "Administrator");

            await EnsureUserInRoleAsync(userManager, superAdmin, IdentityRoleNames.SuperAdmin);
            await EnsureUserInRoleAsync(userManager, admin, IdentityRoleNames.Admin);
        }

        private static string GetRequiredSetting(IConfiguration configuration, string key)
        {
            var value = configuration[key];

            if (string.IsNullOrWhiteSpace(value))
                throw new InvalidOperationException(
                    $"Required configuration value '{key}' is missing or empty.");

            return value;
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
