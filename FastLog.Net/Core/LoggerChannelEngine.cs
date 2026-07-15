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
        /// The task list for execute all agents in parallel.
        /// </summary>



        /// <summary>
        /// Start the FastLog.Net logger core engine.
        /// </summary>
        /// <returns></returns>

        public void StartLogger()
        {
            lock (LoggerLifecycleSync)
            {
                if (IsLoggerRunning)
                {
                    return;
                }

                if (_cts.IsCancellationRequested)
                {
                    throw new InvalidOperationException("The logger can not be restarted after it has been stopped.");
                }

                if (!Agents.AgentList.Any())
                {
                    throw new InvalidOperationException("The logger can not start with no logging agent.");
                }

                TaskCompletionSource<bool> IsEngineRunning = new TaskCompletionSource<bool>();

                // Logger core engine -> ( Channel Prodecure / Consumer approach )
                LoggerEngineTask = Task.Run(() => RunLoggerEngineAsync(IsEngineRunning));

                // Wait here for releasing signal from the "TaskCompletionSource".( after the engine run successfully)
                // Otherwise the method execution will be fisnihed BEFORE the engine starts successfully.
                IsEngineRunning.Task.Wait();
            }

            try
            {
                InternalLogger?.LogInternalSystemEvent(new LogEventModel(Enums.LogEventTypes.SYSTEM, "FastLog.Net engine has been started."));
            }
            catch
            {
                // Ignore the exceptions , because if the Internal Logger itself throws an exception we can not do anything about that !
            }


        }


        private async Task RunLoggerEngineAsync(TaskCompletionSource<bool> isEngineRunning)
        {
            try
            {
                IsLoggerRunning = true;
                isEngineRunning.TrySetResult(true); // Release the waiting thread to go on !!

                while (!(LoggerChannelReader.Completion.IsCompleted || _cts.IsCancellationRequested))
                {
                    QueuedLogEvent queuedLogEvent = null;

                    try
                    {
                        // Awaiting for a log event to be put in the channel...
                        queuedLogEvent = await LoggerChannelReader.ReadAsync(_cts.Token)
                                                                 .ConfigureAwait(false);

                        ILogEventModel EventModelFromChannel = queuedLogEvent?.LogEvent;

                        if (EventModelFromChannel == null) continue;


                        HandleInMemoryEvents(EventModelFromChannel);

                        // Consume the LogEventModel on channel one by one with each logger agent in the agent list !


                        // Pararllel.Foreach has been tested BUT normal foreach and Task.WhenAll is faster thean Parallel.Foreach in this case.


                        await ProcessEventWithAllAgentsAsync(EventModelFromChannel).ConfigureAwait(false);


                        // Just for sure !! in fact never gonna happen ! long Max value is "9,223,372,036,854,775,807"

                        if (queueProcessedEventCount >= long.MaxValue)
                        {
                            queueProcessedEventCount = 0;
                        }

                        queueProcessedEventCount++;



                        HandleEvents(EventModelFromChannel);




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
                    finally
                    {
                        if (queuedLogEvent != null)
                        {
                            Volatile.Write(ref lastCompletedLoggerChannelSequence, queuedLogEvent.Sequence);
                        }
                    }

                }
            }
            catch (Exception ex)
            {
                isEngineRunning.TrySetException(ex);
                InternalLogger?.LogInternalException(ex);
            }
            finally
            {
                IsLoggerRunning = false;

                if (!isEngineRunning.Task.IsCompleted)
                {
                    isEngineRunning.TrySetCanceled();
                }
            }
        }


        public async Task ProcessAllEventsInQueue()
        {
            long targetSequence;

            lock (LoggerChannelWriteSync)
            {
                targetSequence = Volatile.Read(ref lastEnqueuedLoggerChannelSequence);
            }

            // Wait until every event that was queued before this method was called has either
            // completed processing or was removed by the channel's configured DropOldest policy.
            while (Volatile.Read(ref lastCompletedLoggerChannelSequence) < targetSequence)
            {
                await Task.Delay(1, _cts.Token).ConfigureAwait(false);
            }

        }

        public void StopLogger()
        {
            lock (LoggerLifecycleSync)
            {
                IsLoggerRunning = false;

                if (!_cts.IsCancellationRequested)
                {
                    _cts.Cancel();
                }
            }

            try
            {
                InternalLogger?.LogInternalSystemEvent(new LogEventModel(Enums.LogEventTypes.SYSTEM, "FastLog.Net engine has been stopped.",
                                                                                                                "Via StopLogger() method."));
            }
            catch
            {
                // Ignore the exceptions , because if the Internal Logger itself throws an exception we can not do anything about that !
            }
        }

        public void ResetStatistics()
        {
            queueProcessedEventCount = 0;
            queueTotalEventCount = 0;
        }

        private async Task ProcessEventWithAllAgentsAsync(ILogEventModel eventModel)
        {
            Lazy<List<Task>> LazyTaskList = new Lazy<List<Task>>();


            foreach (IAgent logger in Agents.AgentList)
            {

                if (logger is null)
                {
                    continue;
                }

                if (!IsLoggerRunning) return;


                try
                {
                    if (string.IsNullOrWhiteSpace(eventModel.EventMessage)) continue;



                    // Important Warning : "Task.WhenAll" has serious performance issue here ,
                    // so use it just when we have more than 1 agent and/or an hevy IO waiting operation is used like Email/SMS send , HTTP operation and etc.

                    if (Configuration.RunAgentsInParallel)
                    {
                        LazyTaskList.Value.Add(logger.ExecuteAgent(eventModel, _cts.Token));

                    }
                    else
                    {
                        await logger.ExecuteAgent(eventModel, _cts.Token).ConfigureAwait(false);
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


            if (Configuration.RunAgentsInParallel)
            {
                // Await until all tasks have been fisnihed.
                await Task.WhenAll(LazyTaskList.Value).ConfigureAwait(false);
            }


        }


        private void HandleEvents(ILogEventModel eventModel)
        {
            // Raise the event
            try
            {
                OnEventProcessed?.Invoke(this, eventModel);
            }
            catch (Exception ex)
            {
                InternalLogger?.LogInternalException(ex);
            }
        }


    }
}
