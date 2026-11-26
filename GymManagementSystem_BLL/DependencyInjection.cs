using GymManagementSystem_BLL.Attachments;
using GymManagementSystem_BLL.Services;
using Microsoft.Extensions.DependencyInjection;

namespace GymManagementSystem_BLL
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddGymBusinessServices(this IServiceCollection services)
        {
            services.AddScoped<IPlanService, PlanService>();
            services.AddScoped<IMemberService, MemberService>();
            services.AddScoped<ISessionService, SessionService>();
            services.AddScoped<ITrainerService, TrainerService>();
            services.AddScoped<IAnalyticsService, AnalyticsService>();
            services.AddScoped<IAttachmentService, AttachmentService>();

            services.AddSingleton(TimeProvider.System);

            services.AddAutoMapper(cfg => cfg.AddMaps(typeof(DependencyInjection).Assembly));

            return services;
        }
    }
}
