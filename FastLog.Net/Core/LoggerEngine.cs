﻿using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using TrendSoft.FastLog.Interfaces;
using TrendSoft.FastLog.Models;

namespace TrendSoft.FastLog.Core
{

    public partial class Logger : IDisposable
    {

        public Task StartLogger()
        {
      

            List<Task> tasksList = null;

            if (this._runAgentsInParallel) tasksList = new List<Task>();

            // Logger engine ->

            return Task.Run((Func<Task>)(async () =>
            {


                while (!LoggerChannelReader.Completion.IsCompleted && !_cts.IsCancellationRequested)
                {
                    _IsLoggerRunning = true;

                    LogEventModel EventModelFromChannel = await LoggerChannelReader.ReadAsync().ConfigureAwait(false);

                    if (EventModelFromChannel != null)
                    {

                        // Consume the LogEventModel on channel one by one with each logger agent in the agent list !

                        foreach (ILoggerAgent logger in _loggerAgents)
                        {
                            if (logger is null)
                            {
                                continue;
                            }

                            try
                            {
                                if (!string.IsNullOrWhiteSpace(EventModelFromChannel.LogText))
                                {
                                    if (this._runAgentsInParallel)
                                    {
                                        tasksList.Add(logger.LogEvent(EventModelFromChannel, _cts.Token));
                                    }
                                    else
                                    {
                                        await logger.LogEvent(EventModelFromChannel, _cts.Token).ConfigureAwait(false);
                                    }
                                }
                                else
                                {
                                    continue;
                                }

                            }
                            catch (Exception ex)
                            {
                                _internalLogger?.LogInternalException(ex);
                            }

                        }

                        if (this._runAgentsInParallel)
                        {
                            await Task.WhenAll(tasksList).ConfigureAwait(false);
                        }

                    }

                }



            }));
        }


        public void StopLogger()
        {
            _IsLoggerRunning = true;
            _cts.Cancel();
        }


    }
}
