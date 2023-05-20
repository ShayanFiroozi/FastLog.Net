using System;

namespace FastLog.Helpers.ExtendedMethods
{
    internal static class DateTimeExtendedMethods
    {
        public static string ToLogFriendlyDateTime(this DateTime dateTime) => dateTime.ToString("yyyy-MM-dd HH:mm:ss");
    }
}
