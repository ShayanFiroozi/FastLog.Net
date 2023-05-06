﻿using System;
using System.Collections.Concurrent;
using System.Threading.Tasks;

namespace LogModule
{

    public class Logger : IDisposable, ILogger
    {

        //private LiteDatabase logDB = null;
        private ConcurrentBag<ILoggerAgent> _loggingAgents = new();



        #region Constructors

        public Logger()
        {
            // Delete the inner exception log file if reaches the LOG_FILE_MAX_SIZE_IN_MB

            try
            {

                InnerException.InternalException.DeleteInnerExceptionLogFile();
            }
            catch
            {

            }
        }

        #endregion



        #region RegistrationMethods
        public void AddLoggingAgent(ILoggerAgent logger)
        {
            _loggingAgents.Add(logger);


            //if (logger is IDBLogger && logDB == null)
            //{
            //    logDB = new LiteDatabase(logger.LogFile);
            //}

        }

        public void ClearLoggingAgents()
        {
            _loggingAgents.Clear();


        }

#if Test
#warning Be Careful ---> Test Mode is On !
#endif

        #endregion



        #region LoggingMethod

        public void LogInfo(string LogText,
                            string ExtraInfo = "",
                            string Source = "")
        {
            try
            {
                _executeLogging(new LogMessage(LogMessage.LogTypeEnum.INFO, LogText, ExtraInfo, Source));
            }
            catch (Exception ex)
            {
                InnerException.InternalException.LogInnerException(ex);
            }
        }


        public Task LogInfoTask(string LogText,
                            string ExtraInfo = "",
                            string Source = "")
        {
            return Task.Run(() => LogInfo(LogText, ExtraInfo, Source));
        }



        public void LogWarning(string LogText,
                            string ExtraInfo = "",
                            string Source = "")
        {
            try
            {
                _executeLogging(new LogMessage(LogMessage.LogTypeEnum.WARNING, LogText, ExtraInfo, Source));
            }
            catch (Exception ex)
            {
                InnerException.InternalException.LogInnerException(ex);
            }
        }


        public Task LogWarningTask(string LogText,
                           string ExtraInfo = "",
                           string Source = "")
        {
            return Task.Run(() => LogWarning(LogText, ExtraInfo, Source));
        }






        public void LogError(string LogText,
                            string ExtraInfo = "",
                            string Source = "")
        {
            try
            {
                _executeLogging(new LogMessage(LogMessage.LogTypeEnum.ERROR, LogText, ExtraInfo, Source));
            }
            catch (Exception ex)
            {
                InnerException.InternalException.LogInnerException(ex);
            }
        }


        public Task LogErrorTask(string LogText,
                          string ExtraInfo = "",
                          string Source = "")
        {
            return Task.Run(() => LogError(LogText, ExtraInfo, Source));
        }







        public void LogException(Exception exception)
        {

            if (exception == null)
            {
                return;
            }

            try
            {
                _executeLogging(new LogMessage(LogMessage.LogTypeEnum.EXCEPTION,
                                               " Message : " + exception.Message ?? "-",
                                               " InnerMessage : " + (exception.InnerException?.Message ?? "-") +
                                               " , " +
                                               " StackTrace : " + (exception.StackTrace ?? "-"),
                                                 (exception.Source ?? "-")));
            }
            catch (Exception ex)
            {
                InnerException.InternalException.LogInnerException(ex);
            }
        }



        public Task LogExceptionTask(Exception exception)
        {
            return Task.Run(() => LogException(exception));
        }





        public void LogDebug(string LogText,
                          string ExtraInfo = "",
                          string Source = "")
        {
            try
            {
                _executeLogging(new LogMessage(LogMessage.LogTypeEnum.DEBUG, LogText, ExtraInfo, Source));
            }
            catch (Exception ex)
            {
                InnerException.InternalException.LogInnerException(ex);
            }
        }

        public Task LogDebugTask(string LogText,
                              string ExtraInfo = "",
                              string Source = "")
        {
            return Task.Run(() => LogDebug(LogText, ExtraInfo, Source));
        }





        public void LogLoggerInternalException(Exception exception)
        {

            if (exception == null)
            {
                return;
            }

            try
            {
                InnerException.InternalException.LogInnerException(exception);
            }
            catch (Exception)
            {

            }
        }



        public Task LogLoggerInternalExceptionTask(Exception exception)
        {
            return Task.Run(() => LogLoggerInternalException(exception));
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
                    _loggingAgents = null;
                }

                catch (Exception)
                {
                    // InnerException.InnerException.LogInnerException(ex);
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

        //~Logger()
        //{
        //    Dispose(disposing: false);
        //}
        #endregion



        #region PrivateMethods
        private void _executeLogging(LogMessage LogMessage)
        {
            if (LogMessage is null)
            {
                return;
            }

            foreach (ILoggerAgent _logger in _loggingAgents)
            {
                if (_logger is null)
                {
                    continue;
                }

                try
                {

                    if (_logger is IFileLogger)
                    {
                        ((IFileLogger)_logger)?.SaveLog(logMessage: LogMessage);
                    }
                    //else if (_logger is IDBLogger)
                    //{
                    //    ((IDBLogger)_logger)?.SaveLog(logMessage: LogMessage, logDB: logDB); // inject the logDB
                    //}
                }
                catch (Exception ex)
                {
                    InnerException.InternalException.LogInnerException(ex);
                }
            }


        }
        #endregion



    }
}
