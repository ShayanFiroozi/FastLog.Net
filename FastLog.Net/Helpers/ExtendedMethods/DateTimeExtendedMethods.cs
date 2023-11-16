/*---------------------------------------------------------------------------------------------

                ► FastLog.Net , High Performance Logger For .Net ◄



 → Copyright (c) 2020-2023 Shayan Firoozi , Bandar Abbas , Iran , Under MIT License.

 → Contact : <shayan.firoozi@gmail.com>

 → GitHub repository : https://github.com/ShayanFiroozi/FastLog.Net

---------------------------------------------------------------------------------------------*/

using System;



namespace FastLog.Helpers.ExtendedMethods
{

    /// <summary>
    /// DateTime class extended method.
    /// </summary>
    internal static class DateTimeExtendedMethods
    {
        public static string ToFriendlyDateTime(this DateTime dateTime, bool ToUTC = false) => $"{ToFriendlyDate(dateTime, ToUTC)} {ToFriendlyTime(dateTime, ToUTC)}";

        public static string ToFriendlyDate(this DateTime dateTime, bool ToUTC = false) => !ToUTC ? dateTime.ToString("yyyy-MM-dd") : dateTime.ToUniversalTime().ToString("yyyy-MM-dd");

        public static string ToFriendlyTime(this DateTime dateTime, bool ToUTC = false) => !ToUTC ? dateTime.ToString("HH:mm:ss") : dateTime.ToUniversalTime().ToString("HH:mm:ss");




    }
}
