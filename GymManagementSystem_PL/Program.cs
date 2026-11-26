using AutoMapper;
using GymManagementSystem_BLL;
using GymManagementSystem_DAL;
using GymManagementSystem_DAL.Data;
using GymManagementSystem_DAL.Data.Identity;
using GymManagementSystem_DAL.Data.Seed;
using GymManagementSystem_PL.Logging;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Serilog;
using Serilog.Debugging;

SelfLog.Enable(message => Console.Error.WriteLine(message));

Log.Logger = new LoggerConfiguration()
    .WriteTo.Console()
    .CreateBootstrapLogger();

try
{
    var builder = WebApplication.CreateBuilder(args);

    builder.Host.UseSerilog((context, services, configuration) =>
    {
        configuration
            .ReadFrom.Configuration(context.Configuration)
            .ReadFrom.Services(services)
            .Enrich.FromLogContext()
            .WriteTo.Console();

        var loggingConnection = context.Configuration.GetConnectionString("LoggingConnection");

        if (!string.IsNullOrWhiteSpace(loggingConnection))
        {
            configuration.WriteTo.PostgreSQL(
                loggingConnection,
                "logs",
                LogColumnWriters.Default,
                needAutoCreateTable: false,
                period: TimeSpan.FromSeconds(5),
                batchSizeLimit: 30);
        }
    });

    // Add services to the container.
    builder.Services.AddControllersWithViews();

    builder.Services
        .AddGymDataAccess(builder.Configuration)
        .AddGymBusinessServices();

    builder.Services.AddIdentity<GymUser, GymRole>(config =>
    {

    }).AddEntityFrameworkStores<GymDbContext>()
    .AddDefaultTokenProviders();

    var app = builder.Build();

    if (app.Environment.IsDevelopment())
    {
        await using var scope = app.Services.CreateAsyncScope();
        var dbContext = scope.ServiceProvider.GetRequiredService<GymDbContext>();
        var userManager = scope.ServiceProvider.GetRequiredService<UserManager<GymUser>>();
        var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<GymRole>>();
        await dbContext.Database.MigrateAsync();
        await DatabaseSeeder.SeedAllAsync(userManager, roleManager, app.Configuration, dbContext);
        scope.ServiceProvider.GetRequiredService<IMapper>().ConfigurationProvider.AssertConfigurationIsValid();
    }

    app.UseSerilogRequestLogging();
    app.UseRouting();
    app.MapStaticAssets();

    app.MapControllerRoute(
        name: "default",
        pattern: "{controller=Home}/{action=Index}/{id?}")
        .WithStaticAssets();


    app.Run();
}
catch (Exception ex) when (ex.GetType().Name is not "HostAbortedException")
{
    Log.Fatal(ex, "Application terminated unexpectedly");
}
finally
{
    Log.CloseAndFlush();
}