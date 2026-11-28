using Npgsql;

namespace GymManagementSystem_PL.Logging
{
    public sealed class LogRetentionService(IConfiguration configuration,
        ILogger<LogRetentionService> logger) : BackgroundService
    {
        private static readonly TimeSpan Interval = TimeSpan.FromHours(24);

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            var connectionString = configuration.GetConnectionString("LoggingConnection");
            if (string.IsNullOrWhiteSpace(connectionString)) return;

            var retentionDays = configuration.GetValue("LogRetention:Days", 30);

            if (retentionDays <= 0)
            {
                logger.LogWarning(
                    "Log retention cleanup skipped because LogRetention:Days must be greater than zero.");

                return;
            }

            try
            {
                using var timer = new PeriodicTimer(Interval);
                do
                {
                    await PurgeAsync(connectionString, retentionDays, stoppingToken);
                }
                while (await timer.WaitForNextTickAsync(stoppingToken));
            }
            catch (OperationCanceledException)
            {
                throw;
            }
        }

        private async Task PurgeAsync(string connectionString, int retentionDays, CancellationToken stoppingToken)
        {
            try
            {
                await using var connection = new NpgsqlConnection(connectionString);
                await connection.OpenAsync(stoppingToken);

                await using var command = new NpgsqlCommand(
                    """DELETE FROM logs WHERE raise_date < now() - make_interval(days => @days)""",
                    connection);
                command.Parameters.AddWithValue("days", retentionDays);

                var deleted = await command.ExecuteNonQueryAsync(stoppingToken);
                logger.LogInformation("Log retention: deleted {Count} rows older than {Days} days", deleted, retentionDays);
            }
            catch (Exception ex) when (ex is not OperationCanceledException)
            {
                logger.LogWarning(ex, "Log retention cleanup failed");
            }
        }
    }
}
