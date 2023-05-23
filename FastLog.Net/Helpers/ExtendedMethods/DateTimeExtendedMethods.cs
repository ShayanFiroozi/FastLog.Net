/*---------------------------------------------------------------------------------------------

                ► FastLog.Net , High Performance Logger For .Net ◄



 → Copyright (c) 2020-2023 Shayan Firoozi , Bandar Abbas , Iran , Under MIT License.

 → Contact : <shayan.firoozi@gmail.com>

 → GitHub repository : https://github.com/ShayanFiroozi/FastLog.Net

---------------------------------------------------------------------------------------------*/

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
