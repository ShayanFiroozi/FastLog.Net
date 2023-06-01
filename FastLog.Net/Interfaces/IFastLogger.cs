/*---------------------------------------------------------------------------------------------

                ► FastLog.Net , High Performance Logger For .Net ◄



 → Copyright (c) 2020-2023 Shayan Firoozi , Bandar Abbas , Iran , Under MIT License.

 → Contact : <shayan.firoozi@gmail.com>

 → GitHub repository : https://github.com/ShayanFiroozi/FastLog.Net

---------------------------------------------------------------------------------------------*/

using System;
using System.Threading.Tasks;

namespace FastLog.Interfaces
{
    public interface IFastLogger
    {

#if NETFRAMEWORK || NETSTANDARD2_0

        ValueTask LogInfo(string LogText, string Details = "", int EventId = 0);

        ValueTask LogNote(string LogText, string Details = "", int EventId = 0);

        ValueTask LogTodo(string LogText, string Details = "", int EventId = 0);

        ValueTask LogWarning(string LogText, string Details = "", int EventId = 0);

        ValueTask LogAlert(string LogText, string Details = "", int EventId = 0);

        ValueTask LogError(string LogText, string Details = "", int EventId = 0);

        ValueTask LogDebug(string LogText, string Details = "", int EventId = 0);

        ValueTask LogException(Exception exception, int EventId = 0);

        ValueTask LogSystem(string LogText, string Details = "", int EventId = 0);

        ValueTask LogSecurity(string LogText, string Details = "", int EventId = 0);

        ValueTask LogConsole(string LogText, string Details = "", int EventId = 0);
#else

        public ValueTask LogInfo(string LogText, string Details = "", int EventId = 0);

        public ValueTask LogNote(string LogText, string Details = "", int EventId = 0);

        public ValueTask LogTodo(string LogText, string Details = "", int EventId = 0);

        public ValueTask LogWarning(string LogText, string Details = "", int EventId = 0);

        public ValueTask LogAlert(string LogText, string Details = "", int EventId = 0);

        public ValueTask LogError(string LogText, string Details = "", int EventId = 0);

        public ValueTask LogDebug(string LogText, string Details = "", int EventId = 0);

        public ValueTask LogException(Exception exception, int EventId = 0);

        public ValueTask LogSystem(string LogText, string Details = "", int EventId = 0);

        public ValueTask LogSecurity(string LogText, string Details = "", int EventId = 0);

        public ValueTask LogConsole(string LogText, string Details = "", int EventId = 0);

#endif
    }
}
