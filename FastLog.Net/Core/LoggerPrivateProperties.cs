/*---------------------------------------------------------------------------------------------

                ► FastLog.Net , High Performance Logger For .Net ◄



 → Copyright (c) 2020-2023 Shayan Firoozi , Bandar Abbas , Iran , Under MIT License.

 → Contact : <shayan.firoozi@gmail.com>

 → GitHub repository : https://github.com/ShayanFiroozi/FastLog.Net

---------------------------------------------------------------------------------------------*/

using FastLog.Interfaces;
using FastLog.Internal;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Channels;

namespace FastLog.Core
{

    public sealed partial class Logger : IDisposable
    {



        /// <summary>
        /// Global cancelation token for logger.
        /// </summary>
        private readonly CancellationTokenSource _cts = new CancellationTokenSource();

        /// <summary>
        /// Internal Logger agent to log the FastLog.Net internal exception or events.
        /// </summary>
        private InternalLogger InternalLogger = null;

        internal ConfigManager Configuration;

        private bool IsLoggerRunning = false;

        /// <summary>
        /// Active agent(s) defined in the logger agent list.
        /// </summary>
        private AgentsManager Agents { get; set; }

        private List<ILogEventModel> inMemoryEvents { get; } = new List<ILogEventModel>();

        private long queueTotalEventCount = 0;

        private long queueProcessedEventCount = 0;


        #region Channel Properties

        /// <summary>
        /// If more than "LoggerChannelMaxCapacity" event(s) placed into the channel , the cannel will drop the oldest and then add the new one.
        /// </summary>
        private const int LoggerChannelMaxCapacity = 1_000_000;

        private readonly Channel<ILogEventModel> LoggerChannel =
                   Channel.CreateBounded<ILogEventModel>(new BoundedChannelOptions(LoggerChannelMaxCapacity)
                   { SingleReader = true, FullMode = BoundedChannelFullMode.DropOldest });

        private readonly ChannelReader<ILogEventModel> LoggerChannelReader;
        private readonly ChannelWriter<ILogEventModel> LoggerChannelWriter;
        #endregion




    }
}
