using GymManagementSystem_DAL.Data;
using GymManagementSystem_DAL.Data.Interceptors;
using GymManagementSystem_DAL.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace GymManagementSystem_DAL
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddGymDataAccess(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddSingleton<BaseEntityInterceptor>();

            var connectionString = configuration.GetConnectionString("DefaultConnection")
                ?? throw new InvalidOperationException("Connection string 'DefaultConnection' was not found.");

            services.AddDbContext<GymDbContext>((sp, options) =>
            {
                options.UseSqlServer(connectionString)
                       .AddInterceptors(sp.GetRequiredService<BaseEntityInterceptor>());
            });

            services.AddScoped<IUnitOfWork, UnitOfWork>();

            return services;
        }
    }
}
