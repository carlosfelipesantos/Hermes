namespace Hermes.Utils
{
    public static class TimeHelper
    {
        private static readonly TimeZoneInfo _brasiliaTimeZone =
            TimeZoneInfo.FindSystemTimeZoneById("E. South America Standard Time");

        public static DateTime Now => TimeZoneInfo.ConvertTime(DateTime.UtcNow, _brasiliaTimeZone);
    }
}