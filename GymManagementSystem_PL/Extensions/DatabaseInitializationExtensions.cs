using AutoMapper;
using GymManagementSystem_DAL.Data;
using GymManagementSystem_DAL.Data.Seed;
using Microsoft.EntityFrameworkCore;

namespace GymManagementSystem_PL.Extensions
{
    public static class DatabaseInitializationExtensions
    {
        public static async Task MigrateAndSeedAsync(this WebApplication app)
        {
            await using var scope = app.Services.CreateAsyncScope();
            var services = scope.ServiceProvider;

            services.GetRequiredService<IMapper>().ConfigurationProvider.AssertConfigurationIsValid();

            await services.GetRequiredService<GymDbContext>().Database.MigrateAsync();
            await DatabaseSeeder.SeedAsync(services);
        }
    }
}
