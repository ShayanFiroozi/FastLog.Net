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

            if (ConfigManager.runAgentsInParallel) tasksList = new List<Task>();

            // Logger engine ->

            return Task.Run(async () =>
            {

                while (!LoggerChannelReader.Completion.IsCompleted && !_cts.IsCancellationRequested)
                {
                    _IsLoggerRunning = true;

                    LogEventModel EventModelFromChannel = await LoggerChannelReader.ReadAsync().ConfigureAwait(false);

                    if (EventModelFromChannel != null)
                    {

                        // Consume the LogEventModel on channel one by one with each logger agent in the agent list !

                        foreach (ILoggerAgent logger in agentsManager.AgentList)
                        {
                            if (logger is null)
                            {
                                continue;
                            }
                            
                            if (!_IsLoggerRunning) return;

                            try
                            {
                                if (!string.IsNullOrWhiteSpace(EventModelFromChannel.EventMessage))
                                {
                                    if (ConfigManager.runAgentsInParallel)
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

                        if (ConfigManager.runAgentsInParallel)
                        {
                            await Task.WhenAll(tasksList).ConfigureAwait(false);
                        }

                    }

                }



            });
        }


        public void StopLogger()
        {
            _IsLoggerRunning = false;
            _cts.Cancel();
        }


    }
}
