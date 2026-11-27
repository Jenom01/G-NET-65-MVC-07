using GymManagementSystem_DAL.Data.Identity;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;

namespace GymManagementSystem_DAL.Data.Seed
{
    public static class DatabaseSeeder
    {
        public static async Task SeedAsync(IServiceProvider services)
        {
            ArgumentNullException.ThrowIfNull(services);

            var dbContext = services.GetRequiredService<GymDbContext>();

            var identityOptions = services.GetRequiredService<IOptions<IdentitySeedOptions>>().Value;

            await IdentitySeeder.SeedAsync(
                services.GetRequiredService<UserManager<GymUser>>(),
                services.GetRequiredService<RoleManager<GymRole>>(),
                identityOptions);

            await PlanSeeder.SeedAsync(dbContext);
            await CategorySeeder.SeedAsync(dbContext);
            await MemberSeeder.SeedAsync(dbContext);
            await TrainerSeeder.SeedAsync(dbContext);
            await MembershipSeeder.SeedAsync(dbContext);
            await SessionSeeder.SeedAsync(dbContext);
            await BookingSeeder.SeedAsync(dbContext);
        }
    }
}
