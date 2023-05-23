﻿/*---------------------------------------------------------------------------------------------

                ► FastLog.Net , High Performance Logger For .Net ◄



 → Copyright (c) 2020-2023 Shayan Firoozi , Bandar Abbas , Iran , Under MIT License.

 → Contact : <shayan.firoozi@gmail.com>

 → GitHub repository : https://github.com/ShayanFiroozi/FastLog.Net

---------------------------------------------------------------------------------------------*/

using FastLog.Interfaces;
using FastLog.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace FastLog.Core
{

    /// <summary>
    /// The logger engine which consume and execute the produced log event in the channel.
    /// Note : The FastLog.Net uses producer/consumer pattern of System.Threading.Channel
    /// For more info visit : https://code-maze.com/dotnet-producer-consumer-channels/
    /// </summary>
    public sealed partial class Logger : IDisposable
    {
       /// <summary>
       /// Start the FastLog.Net logger core engine.
       /// </summary>
       /// <returns></returns>

        public Task StartLogger()
        {

            List<Task> tasksList = null;

            if (Configuration.RunAgentsInParallel) tasksList = new List<Task>();

            // Logger engine ->

            return Task.Run(async () =>
            {

                while (!LoggerChannelReader.Completion.IsCompleted && !_cts.IsCancellationRequested)
                {

                    try
                    {
                        IsLoggerRunning = true;

                        LogEventModel EventModelFromChannel = await LoggerChannelReader.ReadAsync().ConfigureAwait(false);

                        if (EventModelFromChannel != null)
                        {
                            
                            HandleInMemoryEvents(EventModelFromChannel);


                            // Consume the LogEventModel on channel one by one with each logger agent in the agent list !

                            foreach (IAgent logger in Agents.AgentList)
                            {
                                if (logger is null)
                                {
                                    continue;
                                }

                                if (!IsLoggerRunning) return;

                                try
                                {
                                    if (!string.IsNullOrWhiteSpace(EventModelFromChannel.EventMessage))
                                    {
                                        if (Configuration.RunAgentsInParallel)
                                        {
                                            tasksList.Add(logger.ExecuteAgent(EventModelFromChannel, _cts.Token));
                                        }
                                        else
                                        {
                                            await logger.ExecuteAgent(EventModelFromChannel, _cts.Token).ConfigureAwait(false);
                                        }
                                    }
                                    else
                                    {
                                        continue;
                                    }

                                }
                                catch (Exception ex)
                                {
                                    this.InternalLogger?.LogInternalException(ex);
                                }

                                //Console.WriteLine($"{LoggerChannelReader.Count:N0} item(s) left in channel.");
                            }

                            if (Configuration.RunAgentsInParallel)
                            {
                                await Task.WhenAll(tasksList).ConfigureAwait(false);
                            }

                        }

                    }
                    catch (Exception ex)
                    {
                        this.InternalLogger?.LogInternalException(ex);
                    }




                }



            });
        }

        /// <summary>
        /// Check if the number of In Memory Event(s) are greater than "MaxEventsToKeep" property value.
        /// Note : Use "ReaderWriterLockSlim" to lock the object when modifying and making this method Thread-Safe.
        /// Warning : If the count of in-memory-events reached the "MaxEventsToKeep" , this method will drop the oldet event in the list and then add new one.
        /// </summary>
        /// <param name="logEvent">Log Event to store inthe In-Memory-Event list.</param>
        private void HandleInMemoryEvents(LogEventModel logEvent)
        {
              // Enter to the write lock

            _inMemoryEventsLock.EnterWriteLock();

            if (inMemoryEvents.Count == 0) return;

            try
            {

                if (Configuration.MaxEventsToKeep == 0 && inMemoryEvents.Count != 0)
                {
                    inMemoryEvents.Clear();
                    return;
                }

                if (inMemoryEvents.Count >= Configuration.MaxEventsToKeep)
                {
                    inMemoryEvents.RemoveAt(0); // Remove the oldest event.

                }

                inMemoryEvents.Add(logEvent);
            }
            catch (Exception ex)
            {
                InternalLogger?.LogInternalException(ex);
            }
            finally
            {
                // Release the lock
                _inMemoryEventsLock.ExitWriteLock();
            }

        }

        public void StopLogger()
        {
            IsLoggerRunning = false;
            _cts.Cancel();
        }


    }
}
