﻿/*---------------------------------------------------------------------------------------------

                ► FastLog.Net , High Performance Logger For .Net ◄



 → Copyright (c) 2020-2023 Shayan Firoozi , Bandar Abbas , Iran , Under MIT License.

 → Contact : <shayan.firoozi@gmail.com>

 → GitHub repository : https://github.com/ShayanFiroozi/FastLog.Net

---------------------------------------------------------------------------------------------*/

using FastLog.Helpers;
using FastLog.Internal;
using FastLog.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Channels;

namespace FastLog.Core
{

    public sealed partial class Logger : IDisposable
    {



        #region Private Properties

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
        private List<LogEventModel> inMemoryEvents { get; } = new List<LogEventModel>();



        #region Channel Properties

        /// <summary>
        /// If more than LoggerChannelMaxCapacity event placed into the channel , the cannel will drop the oldest and then add the new one.
        /// </summary>
        private const int LoggerChannelMaxCapacity = 1_000_000;

        private readonly Channel<LogEventModel> LoggerChannel =
                   Channel.CreateBounded<LogEventModel>(new BoundedChannelOptions(LoggerChannelMaxCapacity)
                   { SingleReader = true, FullMode = BoundedChannelFullMode.DropOldest });

        private readonly ChannelReader<LogEventModel> LoggerChannelReader;
        private readonly ChannelWriter<LogEventModel> LoggerChannelWriter;
        #endregion

        #endregion



        /// <summary>
        /// Enumerates the last X logged event(s). X = "ConfigManager.MaxEventsToKeep";
        /// </summary>
        public IEnumerable<LogEventModel> InMemoryEvents
        {
            get
            {
                // Grab the lock  
                SlimReadWriteLock.Lock.EnterReadLock();

                try
                {
                    // ToList() is highly necessary here to prevent race condition when accessing inMemoryEvents property.
                    return inMemoryEvents.ToList();
                }
                finally
                {
                    // Release the lock
                    SlimReadWriteLock.Lock.ExitReadLock();
                }

            }
        }




    }
}
