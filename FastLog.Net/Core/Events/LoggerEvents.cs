/*---------------------------------------------------------------------------------------------

                ► FastLog.Net , High Performance Logger For .Net ◄



 → Copyright (c) 2020-2023 Shayan Firoozi , Bandar Abbas , Iran , Under MIT License.

 → Contact : <shayan.firoozi@gmail.com>

 → GitHub repository : https://github.com/ShayanFiroozi/FastLog.Net

---------------------------------------------------------------------------------------------*/

using FastLog.Interfaces;
using FastLog.Models;
using System;

namespace FastLog.Core
{
    public sealed partial class Logger
    {
        public event EventHandler<ILogEventModel> OnEventOccured;

        public event EventHandler<ILogEventModel> OnEventProcessed;

    }
}
