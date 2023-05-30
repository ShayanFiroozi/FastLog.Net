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
using System.Linq;
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

        public void StartLogger()
        {

            TaskCompletionSource<bool> IsEngineRunning = new TaskCompletionSource<bool>();

            // Logger core engine -> ( Channel Prodecure / Consumer approach )

            Task.Run(async () =>
            {

                List<Task> tasksList = null;

                if (Configuration.RunAgentsInParallel) tasksList = new List<Task>();

                while (!LoggerChannelReader.Completion.IsCompleted && !_cts.IsCancellationRequested)
                {

                    try
                    {

                        if (!IsLoggerRunning)
                        {
                            IsLoggerRunning = true;
                            IsEngineRunning.SetResult(true); // Release the waiting thread to go on !!
                        }

                        // Awaiting for a log event to be put in the channel...

                        ILogEventModel EventModelFromChannel = await LoggerChannelReader.ReadAsync(_cts.Token)
                                                                                        .ConfigureAwait(false);
                        // Consume the log event...
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
                                catch (OperationCanceledException)
                                {
                                    InternalLogger?.LogInternalSystemEvent(new LogEventModel(Enums.LogEventTypes.SYSTEM, "FastLog.Net engine has been stopped.",
                                        "Cancelation signal was received."));
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



                            // Just for sure !! in fact never gonna happen ! long Max value is "9,223,372,036,854,775,807"

                            if (queueProcessedEventCount >= long.MaxValue) { queueProcessedEventCount = 0; }
                            queueProcessedEventCount++;

                            // Interlocked.Increment(ref channelProcessedEventCount);

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

                    catch (OperationCanceledException)
                    {
                        InternalLogger?.LogInternalSystemEvent(new LogEventModel(Enums.LogEventTypes.SYSTEM, "FastLog.Net engine has been stopped.",
                            "Cancelation signal was received."));
                    }
                    catch (Exception ex)
                    {
                        InternalLogger?.LogInternalException(ex);
                    }

                }

            }, _cts.Token).ConfigureAwait(false);



            // Wait here for releasing signal from the "TaskCompletionSource".( after the engine run successfully)

            IsEngineRunning.Task.Wait();

            try
            {
                InternalLogger?.LogInternalSystemEvent(new LogEventModel(Enums.LogEventTypes.SYSTEM, "FastLog.Net engine has been started."));
            }
            catch { }


        }

        public async Task ProcessAllEventsInQueue()
        {
            // Wait until all queue's event been processed.

            while (!IsQueueEmpty())
            {
                // Wait until all logs in the queue been processed or cancelation token signal was received.
                 await Task.Delay(0, _cts.Token); 
                //await Task.Yield();
            }

        }

        public void StopLogger()
        {
            IsLoggerRunning = false;
            _cts.Cancel();
        }


    }
}
