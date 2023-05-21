using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using TrendSoft.FastLog.Interfaces;
using TrendSoft.FastLog.Models;

namespace TrendSoft.FastLog.Core
{

    public partial class Logger : IDisposable
    {
        // Channel Producer/Consumer pattern

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

        private void HandleInMemoryEvents(LogEventModel logEvent)
        {
            if (inMemoryEvents.Count >= Configuration.MaxEventsToKeep)
            {
                inMemoryEvents.RemoveAt(0); // Remove the oldest event.

            }

            inMemoryEvents.Add(logEvent);

        }

        public void StopLogger()
        {
            IsLoggerRunning = false;
            _cts.Cancel();
        }


    }
}
