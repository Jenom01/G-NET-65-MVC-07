namespace GymManagementSystem_BLL.Helpers
{
    public static class GymTime
    {
        private static readonly TimeZoneInfo Zone = TimeZoneInfo.FindSystemTimeZoneById("Africa/Cairo");

        public static DateTime ToLocal(DateTime utc)
            => TimeZoneInfo.ConvertTimeFromUtc(DateTime.SpecifyKind(utc, DateTimeKind.Utc), Zone);

        // For datetime-local inputs: same as ToLocal but without seconds
        public static DateTime ToLocalMinute(DateTime utc)
        {
            var local = ToLocal(utc);
            return local.AddTicks(-(local.Ticks % TimeSpan.TicksPerMinute));
        }

        public static bool IsInvalid(DateTime local)
            => Zone.IsInvalidTime(DateTime.SpecifyKind(local, DateTimeKind.Unspecified));

        public static DateTime ToUtc(DateTime local)
            => TimeZoneInfo.ConvertTimeToUtc(DateTime.SpecifyKind(local, DateTimeKind.Unspecified), Zone);
    }
}
