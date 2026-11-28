using GymManagementSystem_BLL;
using GymManagementSystem_DAL;
using GymManagementSystem_PL.Extensions;
using GymManagementSystem_PL.Logging;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
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

    builder.Services.AddHostedService<LogRetentionService>();

    // Add services to the container.
    builder.Services.AddControllersWithViews(options =>
        options.Filters.Add(new AutoValidateAntiforgeryTokenAttribute()));

    builder.Services
        .AddGymDataAccess(builder.Configuration)
        .AddGymBusinessServices()
        .AddGymIdentity(builder.Environment);

    builder.Services.AddAuthorizationBuilder()
        .SetFallbackPolicy(new AuthorizationPolicyBuilder().RequireAuthenticatedUser().Build());

    var app = builder.Build();

    if (app.Environment.IsDevelopment())
    {
        await app.MigrateAndSeedAsync();
    }
    else
    {
        app.UseExceptionHandler("/Home/Error");
        app.UseHsts();

        if (builder.Configuration.GetValue<bool>("RunDatabaseInitialization"))
        {
            await app.MigrateAndSeedAsync();
        }
    }

    app.UseSerilogRequestLogging();

    app.UseHttpsRedirection();

    app.UseRouting();

    app.UseAuthentication();
    app.UseAuthorization();

    app.MapStaticAssets().AllowAnonymous();

    app.MapControllerRoute(
        name: "default",
        pattern: "{controller=Home}/{action=Index}/{id?}")
        .WithStaticAssets();


    app.Run();
}
catch (Exception ex) when (ex is not HostAbortedException)
{
    Log.Fatal(ex, "Application terminated unexpectedly");
}
finally
{
    Log.CloseAndFlush();
}