using NpgsqlTypes;
using Serilog.Sinks.PostgreSQL.ColumnWriters;

namespace GymManagementSystem_PL.Logging
{
    public static class LogColumnWriters
    {
        public static readonly IDictionary<string, ColumnWriterBase> Default =
            new Dictionary<string, ColumnWriterBase>
            {
                { "raise_date",       new TimestampColumnWriter(NpgsqlDbType.TimestampTz) },
                { "level",            new LevelColumnWriter(true, NpgsqlDbType.Text) },
                { "message",          new RenderedMessageColumnWriter(NpgsqlDbType.Text) },
                { "message_template", new MessageTemplateColumnWriter(NpgsqlDbType.Text) },
                { "exception",        new ExceptionColumnWriter(NpgsqlDbType.Text) },
                { "properties",       new LogEventSerializedColumnWriter(NpgsqlDbType.Jsonb) }
            };
    }
}
