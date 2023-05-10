
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Threading.Channels;
using System.Threading.Tasks;
using TrendSoft.LogModule.Interfaces;
using TrendSoft.LogModule.InternalException;
using TrendSoft.LogModule.Models;

namespace TrendSoft.LogModule.Core
{

    public class Logger : IDisposable
    {


        #region Channel Definitions

        private Channel<LogEventModel> LoggerChannel = Channel.CreateUnbounded<LogEventModel>(new UnboundedChannelOptions());

        private ChannelReader<LogEventModel> LoggerChannelReader;
        private ChannelWriter<LogEventModel> LoggerChannelWriter;

        #endregion


        #region Properties

        public IEnumerable<ILoggerAgent> Agents => _loggerAgents;

        private List<ILoggerAgent> _loggerAgents = new();

        public bool ReflectOnConsole { get; private set; } = false;

        public string InternalExceptionsLogFile { get; private set; }

        #endregion



        #region Constructors

        public Logger(string internalExceptionsLogFile,
                      short internalExceptionsLogFileMaxSizeMB = 100,
                      bool reflectOnConsole = false)
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

            ReflectOnConsole = reflectOnConsole;

            // Initialize "Internal Logger Exceptions"
            InternalExceptionLogger.SetLogFile(InternalExceptionsLogFile);
            InternalExceptionLogger.SetLogFileMaxSizeMB(internalExceptionsLogFileMaxSizeMB);

            // Initialize Channels Reader/Writer
            LoggerChannelReader = LoggerChannel.Reader;
            LoggerChannelWriter = LoggerChannel.Writer;
        }

        #endregion



        #region "Logger Agents" management functions

        public void AddLoggingAgent(ILoggerAgent logger) => _loggerAgents.Add(logger);

        public void ClearLoggingAgents() => _loggerAgents.Clear();

        #endregion



        #region LoggingMethod

        public ValueTask LogInfo(string LogText,
                                 string ExtraInfo = "",
                                 string Source = "")
        {
            if (string.IsNullOrWhiteSpace(LogText))
            {
                return ValueTask.CompletedTask;
            }

            try
            {
                return LoggerChannelWriter.WriteAsync(new LogEventModel(LogEventModel.LogTypeEnum.INFO,
                                                                          LogText,
                                                                          ExtraInfo,
                                                                          Source));
            }
            catch (Exception ex)
            {
                InternalExceptionLogger.LogInternalException(ex);
            }

            return ValueTask.CompletedTask;
        }





        public ValueTask LogWarning(string LogText,
                                    string ExtraInfo = "",
                                    string Source = "")
        {
            if (string.IsNullOrWhiteSpace(LogText))
            {
                return ValueTask.CompletedTask;
            }

            try
            {
                return LoggerChannelWriter.WriteAsync(new LogEventModel(LogEventModel.LogTypeEnum.WARNING,
                                                                          LogText,
                                                                          ExtraInfo,
                                                                          Source));
            }
            catch (Exception ex)
            {
                InternalExceptionLogger.LogInternalException(ex);
            }

            return ValueTask.CompletedTask;
        }



        public ValueTask LogError(string LogText,
                                  string ExtraInfo = "",
                                  string Source = "")
        {
            if (string.IsNullOrWhiteSpace(LogText))
            {
                return ValueTask.CompletedTask;
            }

            try
            {
                return LoggerChannelWriter.WriteAsync(new LogEventModel(LogEventModel.LogTypeEnum.ERROR,
                                                                          LogText,
                                                                          ExtraInfo,
                                                                          Source));
            }
            catch (Exception ex)
            {
                InternalExceptionLogger.LogInternalException(ex);
            }

            return ValueTask.CompletedTask;
        }



        public ValueTask LogException(Exception exception)
        {

            if (exception == null)
            {
                return ValueTask.CompletedTask;
            }


            try
            {
                return LoggerChannelWriter.WriteAsync(new LogEventModel(exception));
            }
            catch (Exception ex)
            {
                InternalExceptionLogger.LogInternalException(ex);
            }

            return ValueTask.CompletedTask;


        }




        public ValueTask LogDebug(string LogText,
                          string ExtraInfo = "",
                          string Source = "")
        {
            if (string.IsNullOrWhiteSpace(LogText))
            {
                return ValueTask.CompletedTask;
            }

            try
            {
                return LoggerChannelWriter.WriteAsync(new LogEventModel(LogEventModel.LogTypeEnum.DEBUG,
                                                                          LogText,
                                                                          ExtraInfo,
                                                                          Source));
            }
            catch (Exception ex)
            {
                InternalExceptionLogger.LogInternalException(ex);
            }

            return ValueTask.CompletedTask;
        }





        #endregion



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



        #region Private Functions

        private Task ExecuteLoggingProcess(LogEventModel LogMessage)
        {
            if (LogMessage is null)
            {
                return Task.CompletedTask;
            }

            foreach (ILoggerAgent logger in _loggerAgents)
            {
                if (logger is null)
                {
                    continue;
                }

                try
                {

                    logger?.LogEvent(logMessage: LogMessage);


                }
                catch (Exception ex)
                {
                    InternalExceptionLogger.LogInternalException(ex);
                }
            }


            return Task.CompletedTask;

        }

        #endregion



    }
}
