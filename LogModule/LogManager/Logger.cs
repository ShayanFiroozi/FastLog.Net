using LiteDB;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LogModule
{
    public sealed class Logger : IDisposable
    {

        private LiteDatabase logDB = null;
        private List<ILogger> _loggingAgents = new();




        #region RegistrationMethods
        public void RegisterLoggingAgent(ILogger logger)
        {
            _loggingAgents.Add(logger);


            if (logger is IDBLogger && logDB == null)
            {
                logDB = new LiteDatabase(logger.LogFile);
            }

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
                InnerException.InnerException.LogInnerException(ex);
            }
        }


        public Task LogInfoTaskAsync(string LogText,
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
                InnerException.InnerException.LogInnerException(ex);
            }
        }


        public Task LogWarningTaskAsync(string LogText,
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
                InnerException.InnerException.LogInnerException(ex);
            }
        }


        public Task LogErrorTaskAsync(string LogText,
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
                                               "Message : " + exception.Message ?? "-",
                                               "InnerMessage : " + (exception.InnerException?.Message ?? "-") +
                                               " , " +
                                               "StackTrace : " + (exception.StackTrace ?? "-"),
                                                 (exception.Source ?? "-")));
            }
            catch (Exception ex)
            {
                InnerException.InnerException.LogInnerException(ex);
            }
        }



        public Task LogExceptionTaskAsync(Exception exception)
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
                InnerException.InnerException.LogInnerException(ex);
            }
        }

        public Task LogDebugTaskAsync(string LogText,
                              string ExtraInfo = "",
                              string Source = "")
        {
            return Task.Run(() => LogDebug(LogText, ExtraInfo, Source));
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
                    logDB.Dispose();
                    ClearLoggingAgents();
                    _loggingAgents = null;
                }

                catch (Exception ex)
                {
                    InnerException.InnerException.LogInnerException(ex);
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

            foreach (ILogger _logger in _loggingAgents)
            {
                try
                {

                    if (_logger is IFileLogger)
                    {
                        ((IFileLogger)_logger).SaveLog(LogMessage);
                    }
                    else if (_logger is IDBLogger)
                    {
                        ((IDBLogger)_logger).SaveLog(LogMessage,logDB); // inject the logDB
                    }
                }
                catch (Exception ex)
                {
                    InnerException.InnerException.LogInnerException(ex);
                }
            }
        }
        #endregion



    }
}
