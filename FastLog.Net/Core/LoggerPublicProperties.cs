﻿/*---------------------------------------------------------------------------------------------

                ► FastLog.Net , High Performance Logger For .Net ◄



 → Copyright (c) 2020-2023 Shayan Firoozi , Bandar Abbas , Iran , Under MIT License.

 → Contact : <shayan.firoozi@gmail.com>

 → GitHub repository : https://github.com/ShayanFiroozi/FastLog.Net

---------------------------------------------------------------------------------------------*/

using FastLog.Helpers;
using FastLog.Interfaces;
using FastLog.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace FastLog.Core
{

    public sealed partial class Logger : IDisposable
    {


        /// <summary>
        /// Enumerates the last X logged event(s). X = "ConfigManager.MaxEventsToKeep" value;
        /// </summary>
        public IEnumerable<ILogEventModel> InMemoryEvents
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


        /// <summary>
        /// Count the remaining event(s) in the channel event queue. ( not processed yet !)
        /// </summary>
        public int ChannelEventCount
        {
            get
            {
                try
                {
                    return LoggerChannelReader.CanCount ? LoggerChannelReader.Count : -1;
                }
                catch
                {
                    return -1;
                }
            }
        }



        /// <summary>
        /// Count total event(s) added to channel (include processed and not processed events).
        /// </summary>
        public long ChannelTotalEventCount => channelTotalEventCount; //Interlocked.Read(ref channelTotalEventCount);


        /// <summary>
        /// Count total processed ( executed ) event(s).
        /// </summary>
        public long ChannelProcessedEventCount => channelProcessedEventCount; //Interlocked.Read(ref channelProcessedEventCount);


    }
}
