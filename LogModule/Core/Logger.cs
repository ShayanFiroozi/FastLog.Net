
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

        private Channel<LogMessageModel> LoggerChannel = Channel.CreateUnbounded<LogMessageModel>(new UnboundedChannelOptions());

        private ChannelReader<LogMessageModel> LoggerChannelReader;
        private ChannelWriter<LogMessageModel> LoggerChannelWriter;

        #endregion


        #region Properties

        public IEnumerable<ILoggerAgent> Agents => _loggerAgents;

        private List<ILoggerAgent> _loggerAgents = new();

        public bool ReflectOnConsole { get; private set; } = false;

        #endregion



        #region Constructors

        public Logger(bool reflectOnConsole = false)
        {
            ReflectOnConsole = reflectOnConsole;

            // Initialize "Internal Logger Exceptions"
            InternalExceptionLogger.SetLogFile("LoggerInternalExceptions.log");
            InternalExceptionLogger.SetLogFileMaxSizeMB(100);

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
            try
            {
                return LoggerChannelWriter.WriteAsync(new LogMessageModel(LogMessageModel.LogTypeEnum.INFO,
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
            try
            {
                return LoggerChannelWriter.WriteAsync(new LogMessageModel(LogMessageModel.LogTypeEnum.WARNING,
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
            try
            {
                return LoggerChannelWriter.WriteAsync(new LogMessageModel(LogMessageModel.LogTypeEnum.ERROR,
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
                return LoggerChannelWriter.WriteAsync(new LogMessageModel(LogMessageModel.LogTypeEnum.EXCEPTION,
                                               " Message : " + exception.Message ?? "-",
                                               " InnerMessage : " + (exception.InnerException?.Message ?? "-") +
                                               " , " +
                                               " StackTrace : " + (exception.StackTrace ?? "-"),
                                                 (exception.Source ?? "-")));
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
            try
            {
                return LoggerChannelWriter.WriteAsync(new LogMessageModel(LogMessageModel.LogTypeEnum.DEBUG,
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

        private Task ExecuteLoggingProcess(LogMessageModel LogMessage)
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

                    logger?.SaveLog(logMessage: LogMessage);


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
