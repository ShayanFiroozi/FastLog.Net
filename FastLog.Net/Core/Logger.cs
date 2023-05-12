﻿
using FastLog.Net.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Channels;
using System.Threading.Tasks;
using TrendSoft.FastLog.Agents;
using TrendSoft.FastLog.Interfaces;
using TrendSoft.FastLog.InternalException;
using TrendSoft.FastLog.Models;

namespace TrendSoft.FastLog.Core
{

    public class Logger : IDisposable
    {
        private readonly CancellationTokenSource _cts = new CancellationTokenSource();

        #region Channel Definitions

        private readonly Channel<LogEventModel> LoggerChannel = Channel.CreateUnbounded<LogEventModel>(new UnboundedChannelOptions());

        private readonly ChannelReader<LogEventModel> LoggerChannelReader;
        private readonly ChannelWriter<LogEventModel> LoggerChannelWriter;

        #endregion


        #region Properties

        public IEnumerable<ILoggerAgent> Agents => _loggerAgents;

        private List<ILoggerAgent> _loggerAgents = new List<ILoggerAgent>();

        public string InternalExceptionsLogFile { get; private set; }


        public bool LogMachineName { get; private set; } = false;

        #endregion



        #region Constructors

        public Logger(string internalExceptionsLogFile,
                      short internalExceptionsLogFileMaxSizeMB = 100,
                      bool LogMachineName = false)
        {
            if (internalExceptionsLogFileMaxSizeMB <= 0)
            {
                throw new ArgumentException($"'{nameof(internalExceptionsLogFileMaxSizeMB)}' must be greater than zero.", nameof(internalExceptionsLogFileMaxSizeMB));
            }


            if (string.IsNullOrWhiteSpace(internalExceptionsLogFile))
            {
                throw new ArgumentException($"'{nameof(internalExceptionsLogFile)}' cannot be null or whitespace.", nameof(internalExceptionsLogFile));
            }


            this.LogMachineName = LogMachineName;

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
            if (logger is ConsoleLogger && _loggerAgents.Any(agent => agent is ConsoleLogger))
            {
                throw new Exception("A \"ConsoleLogger\" agent already exists on the agent list.");
            }


#if DEBUG
            if (logger is DebugWindowLogger && _loggerAgents.Any(agent => agent is DebugWindowLogger))
            {
                throw new Exception("A \"DebugWindowLogger\" agent already exists on the agent list.");
            }
#endif


            // Prevent multi File Logger with same log file !
            if (logger is PlainTextFileLogger && _loggerAgents.Any(agent => ((PlainTextFileLogger)agent).LogFile == ((PlainTextFileLogger)logger).LogFile))
            {
                throw new Exception($"A \"PlainTextFileLogger\" agent with the same log file already exists on the agent list.({((PlainTextFileLogger)logger).LogFile})");
            }


            _loggerAgents.Add(logger);
        }

        public void ClearLoggingAgents()
        {
            _loggerAgents.Clear();
        }

        #endregion



        #region Logging Functions


        private ValueTask LogEventHelper(LogEventTypes LogType,
                                         string LogText,
                                         string ExtraInfo = "",
                                         string Source = "")
        {
            if (string.IsNullOrWhiteSpace(LogText))
            {

#if NET5_0_OR_GREATER
                return ValueTask.CompletedTask;
#else
                return default;
#endif

            }

            try
            {
                LogEventModel LogEvent = new LogEventModel(LogType,
                                             LogText,
                                             ExtraInfo,
                                             Source,
                                             LogMachineName);

                return LoggerChannelWriter.WriteAsync(LogEvent);
            }
            catch (Exception ex)
            {
                InternalExceptionLogger.LogInternalException(ex);
            }

#if NET5_0_OR_GREATER
            return ValueTask.CompletedTask;
#else
                return default;
#endif
        }


        private ValueTask LogEventHelper(Exception exception)
        {
            if (exception is null)
            {
#if NET5_0_OR_GREATER
                return ValueTask.CompletedTask;
#else
                return default;
#endif
            }

            try
            {
                LogEventModel LogEvent = new LogEventModel(exception,
                                                           LogMachineName);

                return LoggerChannelWriter.WriteAsync(LogEvent);
            }
            catch (Exception ex)
            {
                InternalExceptionLogger.LogInternalException(ex);
            }

#if NET5_0_OR_GREATER
            return ValueTask.CompletedTask;
#else
                return default;
#endif
        }




        public ValueTask LogInfo(string LogText,
                                 string ExtraInfo = "",
                                 string Source = "")
        {
            return LogEventHelper(LogEventTypes.INFO, LogText, ExtraInfo, Source);
        }



        public ValueTask LogWarning(string LogText,
                                    string ExtraInfo = "",
                                    string Source = "")
        {
            return LogEventHelper(LogEventTypes.WARNING, LogText, ExtraInfo, Source);
        }


        public ValueTask LogAlert(string LogText,
                                string ExtraInfo = "",
                                string Source = "")
        {
            return LogEventHelper(LogEventTypes.ALERT, LogText, ExtraInfo, Source);
        }


        public ValueTask LogError(string LogText,
                                  string ExtraInfo = "",
                                  string Source = "")
        {
            return LogEventHelper(LogEventTypes.ERROR, LogText, ExtraInfo, Source);
        }



        public ValueTask LogDebug(string LogText,
                                  string ExtraInfo = "",
                                  string Source = "")
        {
            return LogEventHelper(LogEventTypes.DEBUG, LogText, ExtraInfo, Source);
        }


        public ValueTask LogException(Exception exception)
        {

            return LogEventHelper(exception);

        }



        public ValueTask LogSystem(string LogText,
                              string ExtraInfo = "",
                              string Source = "")
        {
            return LogEventHelper(LogEventTypes.SYSTEM, LogText, ExtraInfo, Source);
        }



        #endregion






        public Task StartLogger()
        {
            // Logger engine ->

            return Task.Run(async () =>
            {
                

                while (!LoggerChannelReader.Completion.IsCompleted && !_cts.IsCancellationRequested)
                {
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