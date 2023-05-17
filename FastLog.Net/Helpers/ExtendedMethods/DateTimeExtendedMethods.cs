using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FastLog.Net.Helpers.ExtendedMethods
{
    internal static class DateTimeExtendedMethods
    {
        public static string ToLogFriendlyDateTime(this DateTime dateTime) => dateTime.ToString("yyyy/MM/dd HH:mm:ss");
    }
}
