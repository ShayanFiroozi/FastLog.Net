
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Channels;
using System.Threading.Tasks;
using TrendSoft.LogModule.Interfaces;
using TrendSoft.LogModule.InternalException;
using TrendSoft.LogModule.Models;

namespace TrendSoft.LogModule.Core
{

    public class Logger : IDisposable
    {
        private readonly CancellationTokenSource _cts = new();

        #region Channel Definitions

        private readonly Channel<LogEventModel> LoggerChannel = Channel.CreateUnbounded<LogEventModel>(new UnboundedChannelOptions());

        private readonly ChannelReader<LogEventModel> LoggerChannelReader;
        private readonly ChannelWriter<LogEventModel> LoggerChannelWriter;

        #endregion


        #region Properties

        public IEnumerable<ILoggerAgent> Agents => _loggerAgents;

        private List<ILoggerAgent> _loggerAgents = new();

        public string InternalExceptionsLogFile { get; private set; }

        #endregion



        #region Constructors

        public Logger(string internalExceptionsLogFile,
                      short internalExceptionsLogFileMaxSizeMB = 100)
        {
            if (internalExceptionsLogFileMaxSizeMB <= 0)
            {
                throw new ArgumentException($"'{nameof(internalExceptionsLogFileMaxSizeMB)}' must be greater than zero.", nameof(internalExceptionsLogFileMaxSizeMB));
            }


            if (string.IsNullOrWhiteSpace(internalExceptionsLogFile))
            {
                throw new ArgumentException($"'{nameof(internalExceptionsLogFile)}' cannot be null or whitespace.", nameof(internalExceptionsLogFile));
            }

            InternalExceptionsLogFile = internalExceptionsLogFile;


            // Initialize "Internal Logger Exceptions" logger.
            InternalExceptionLogger.SetLogFile(InternalExceptionsLogFile);
            InternalExceptionLogger.SetLogFileMaxSizeMB(internalExceptionsLogFileMaxSizeMB);

            // Initialize Channels Reader/Writer
            LoggerChannelReader = LoggerChannel.Reader;
            LoggerChannelWriter = LoggerChannel.Writer;
        }

        #endregion



        #region "Logger Agents" management functions

        public void AddLoggingAgent(ILoggerAgent logger)
        {
            _loggerAgents.Add(logger);
        }

        public void ClearLoggingAgents()
        {
            _loggerAgents.Clear();
        }

        #endregion



        #region Logging Functions


        private ValueTask LogEventHelper(LogEventModel.LogTypeEnum LogType,
                                         string LogText,
                                         string ExtraInfo = "",
                                         string Source = "")
        {
            if (string.IsNullOrWhiteSpace(LogText))
            {
                return ValueTask.CompletedTask;
            }

            try
            {
                LogEventModel LogEvent = new(LogType,
                                             LogText,
                                             ExtraInfo,
                                             Source);

                return LoggerChannelWriter.WriteAsync(LogEvent);
            }
            catch (Exception ex)
            {
                InternalExceptionLogger.LogInternalException(ex);
            }

            return ValueTask.CompletedTask;
        }


        private ValueTask LogEventHelper(Exception exception)
        {
            if (exception is null)
            {
                return ValueTask.CompletedTask;
            }

            try
            {
                LogEventModel LogEvent = new(exception);

                return LoggerChannelWriter.WriteAsync(LogEvent);
            }
            catch (Exception ex)
            {
                InternalExceptionLogger.LogInternalException(ex);
            }

            return ValueTask.CompletedTask;
        }




        public ValueTask LogInfo(string LogText,
                                 string ExtraInfo = "",
                                 string Source = "")
        {
            return LogEventHelper(LogEventModel.LogTypeEnum.INFO, LogText, ExtraInfo, Source);
        }



        public ValueTask LogWarning(string LogText,
                                   string ExtraInfo = "",
                                   string Source = "")
        {
            return LogEventHelper(LogEventModel.LogTypeEnum.WARNING, LogText, ExtraInfo, Source);
        }


        public ValueTask LogError(string LogText,
                                  string ExtraInfo = "",
                                  string Source = "")
        {
            return LogEventHelper(LogEventModel.LogTypeEnum.ERROR, LogText, ExtraInfo, Source);
        }



        public ValueTask LogDebug(string LogText,
                                  string ExtraInfo = "",
                                  string Source = "")
        {
            return LogEventHelper(LogEventModel.LogTypeEnum.DEBUG, LogText, ExtraInfo, Source);
        }


        public ValueTask LogException(Exception exception)
        {

            return LogEventHelper(exception);

        }



        #endregion






        public Task StartLogger()
        {
            return Task.Run(async () =>
            {

                while (!LoggerChannelReader.Completion.IsCompleted && !_cts.IsCancellationRequested)
                {
                    LogEventModel EventModelFromChannel = await LoggerChannelReader.ReadAsync().ConfigureAwait(false);

                    if (EventModelFromChannel is not null)
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
                                    await logger.LogEvent(EventModelFromChannel, _cts.Token).ConfigureAwait(false);
                                }
                                else
                                {
                                    continue;
                                }


                            }
                            catch (Exception ex)
                            {
                                InternalExceptionLogger.LogInternalException(ex);
                            }

                        }

                    }

                }

            });
        }


        public void StopLogger()
        {
            _cts.Cancel();
        }




        #region DisposeMethods

        private bool disposed = false;

        private void Dispose(bool disposing)
        {
            if (disposed)
            {
                return;
            }

            if (disposing)
            {
                try
                {
                    StopLogger();

                    //logDB?.Dispose();
                    ClearLoggingAgents();
                    _loggerAgents = null;
                }

                catch (Exception ex)
                {
                    InternalExceptionLogger.LogInternalException(ex);
                }
            }

            disposed = true;

        }

        public void Dispose()
        {
            // Do not change this code , Put your clean up code in the Dispose(bool disposing) function instead.
            Dispose(disposing: true);
            GC.SuppressFinalize(this);
        }


        #endregion



    }
}
