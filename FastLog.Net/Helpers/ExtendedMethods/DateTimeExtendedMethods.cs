using System;

namespace FastLog.Helpers.ExtendedMethods
{
    internal static class DateTimeExtendedMethods
    {
        public static string ToFriendlyDateTime(this DateTime dateTime) => $"{ToFriendlyDate(dateTime)} {ToFriendlyTime(dateTime)}";

        public static string ToFriendlyDate(this DateTime dateTime) => dateTime.ToString("yyyy-MM-dd");

        public static string ToFriendlyTime(this DateTime dateTime) => dateTime.ToString("HH:mm:ss");
    }
}
