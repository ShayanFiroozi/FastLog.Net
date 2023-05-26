/*---------------------------------------------------------------------------------------------

                ► FastLog.Net , High Performance Logger For .Net ◄



 → Copyright (c) 2020-2023 Shayan Firoozi , Bandar Abbas , Iran , Under MIT License.

 → Contact : <shayan.firoozi@gmail.com>

 → GitHub repository : https://github.com/ShayanFiroozi/FastLog.Net

---------------------------------------------------------------------------------------------*/

using FastLog.Enums;
using System;

namespace FastLog.Interfaces
{
    public interface ILogEventModel
    {
        DateTime DateTime { get; }
        string Details { get; }
        int EventId { get; }
        string EventMessage { get; }
        LogEventTypes LogEventType { get; }
        Exception Exception { get; }

    }
}