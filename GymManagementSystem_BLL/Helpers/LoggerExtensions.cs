using GymManagementSystem_BLL.Results;
using Microsoft.Extensions.Logging;

namespace GymManagementSystem_BLL.Helpers
{
    public static class LoggerExtensions
    {
        public static Error Rejected(this ILogger logger, string operation, Error error, int? entityId = null)
        {
            logger.LogWarning("{Operation} rejected for {EntityId}: {ErrorCode}", operation, entityId, error.Code);
            return error;
        }
    }
}
