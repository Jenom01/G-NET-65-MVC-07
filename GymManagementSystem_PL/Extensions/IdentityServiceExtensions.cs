using GymManagementSystem_DAL.Data;
using GymManagementSystem_DAL.Data.Identity;
using GymManagementSystem_DAL.Data.Seed;
using Microsoft.AspNetCore.Identity;

namespace GymManagementSystem_PL.Extensions
{
    public static class IdentityServiceExtensions
    {
        public static IServiceCollection AddGymIdentity(this IServiceCollection services,
            IHostEnvironment environment)
        {
            services.AddIdentity<GymUser, GymRole>(options =>
            {
                options.User.RequireUniqueEmail = true;

                options.Password.RequiredLength = 8;
                options.Password.RequireDigit = true;
                options.Password.RequireUppercase = true;
                options.Password.RequireLowercase = true;
                options.Password.RequireNonAlphanumeric = true;

                // Lockout only applies when the login calls PasswordSignInAsync(..., lockoutOnFailure: true)
                options.Lockout.AllowedForNewUsers = true;
                options.Lockout.MaxFailedAccessAttempts = 5;
                options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(15);
            })
                .AddEntityFrameworkStores<GymDbContext>()
                .AddDefaultTokenProviders();

            services.ConfigureApplicationCookie(options =>
            {
                options.LoginPath = "/Account/Login";
                options.AccessDeniedPath = "/Account/AccessDenied";
                options.ExpireTimeSpan = TimeSpan.FromHours(8);
                options.SlidingExpiration = true;
                options.Cookie.HttpOnly = true;
                options.Cookie.SecurePolicy = environment.IsDevelopment()
                    ? CookieSecurePolicy.SameAsRequest
                    : CookieSecurePolicy.Always;
            });

            // Validated when first read (only the seeder reads it), so production doesn't need these keys
            services.AddOptions<IdentitySeedOptions>()
                .BindConfiguration(IdentitySeedOptions.SectionName)
                .ValidateDataAnnotations();

            return services;
        }
    }
}
