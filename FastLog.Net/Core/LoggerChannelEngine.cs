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
        /// The task list for execute all agents in parallel.
        /// </summary>



        /// <summary>
        /// Start the FastLog.Net logger core engine.
        /// </summary>
        /// <returns></returns>

        public void StartLogger()
        {

            TaskCompletionSource<bool> IsEngineRunning = new TaskCompletionSource<bool>();

            // Logger core engine -> ( Channel Prodecure / Consumer approach )

            _ = Task.Run(async () =>
            {


                while (!(LoggerChannelReader.Completion.IsCompleted || _cts.IsCancellationRequested))
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

                }

            }, _cts.Token).ConfigureAwait(false);



            // Wait here for releasing signal from the "TaskCompletionSource".( after the engine run successfully)
            // Otherwise the method execution will be fisnihed BEFORE the engine starts successfully.

            IsEngineRunning.Task.Wait();

            try
            {
                InternalLogger?.LogInternalSystemEvent(new LogEventModel(Enums.LogEventTypes.SYSTEM, "FastLog.Net engine has been started."));
            }
            catch
            {
                // Ignore the exceptions , because if the Internal Logger itself throws an exception we can not do anything about that !
            }


        }

        public async Task ProcessAllEventsInQueue()
        {
            // Wait until all queue's event been processed.

            while (!IsQueueEmpty())
            {
                // Wait until all logs in the queue been processed or cancelation token signal was received.
                await Task.Delay(0, _cts.Token);
            }

        }

        public void StopLogger()
        {
            IsLoggerRunning = false;
            _cts.Cancel();
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
