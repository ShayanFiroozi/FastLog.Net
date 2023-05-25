/*---------------------------------------------------------------------------------------------

                ► FastLog.Net , High Performance Logger For .Net ◄



 → Copyright (c) 2020-2023 Shayan Firoozi , Bandar Abbas , Iran , Under MIT License.

 → Contact : <shayan.firoozi@gmail.com>

 → GitHub repository : https://github.com/ShayanFiroozi/FastLog.Net

---------------------------------------------------------------------------------------------*/

using FastLog.Interfaces;
using FastLog.Models;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading;
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


                        LogEventModel EventModelFromChannel = await LoggerChannelReader.ReadAsync(_cts.Token).ConfigureAwait(false);

                        if (EventModelFromChannel != null)
                        {

                            HandleInMemoryEvents(EventModelFromChannel);



                            // Consume the LogEventModel on channel one by one with each logger agent in the agent list !


                            // Pararllel.Foreach has been tested BUT normal foreach and Task.WhenAll is faster thean Parallel.Foreach in this case.

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
                                        // Important Warning : "Task.WhenAll" has serious performance issue here ,
                                        // so use it just when we have more than 1 agent and/or an hevy IO waiting operation is used like Email/SMS send , HTTP operation and etc.
                                        // "Agents.AgentList.Count() > 1" --> Prevent using "Task.WhenAll" if we have just 1 agent.

                                        if (Configuration.RunAgentsInParallel && Agents.AgentList.Count() > 1)
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
                                    InternalLogger?.LogInternalException(ex);
                                }


                                //Console.WriteLine($"{LoggerChannelReader.Count:N0} item(s) left in channel.");
                            }

                            if (Configuration.RunAgentsInParallel)
                            {
                                await Task.WhenAll(tasksList).ConfigureAwait(false);
                            }




                            // Interlocked.Increment(ref channelProcessedEventCount);
                            channelProcessedEventCount++;

                            // Raise the event
                            try
                            {
                                OnEventProcessed?.Invoke(this, EventModelFromChannel);
                            }
                            catch (Exception ex)
                            {
                                InternalLogger?.LogInternalException(ex);
                            }


                          

                        }


                    }
                    catch (Exception ex)
                    {
                        InternalLogger?.LogInternalException(ex);
                    }

                }



            }, _cts.Token);
        }


        public void StopLogger()
        {
            IsLoggerRunning = false;
            _cts.Cancel();
        }


    }
}
