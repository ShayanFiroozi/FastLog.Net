using System;
using System.Collections.Generic;
using System.IO;
using System.Security.Cryptography;

namespace LogModule
{
    public sealed class Logger : IDisposable
    {

        private List<ILogger> _loggingChannels = new();

       

        #region RegistrationMethods
        public void RegisterLoggingChannel(ILogger logger)
        {
            _loggingChannels.Add(logger);
        }

        public void ClearLoggingChannel()
        {
            _loggingChannels.Clear();
        }

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



        public void LogException(Exception exception)
        {

            if (exception == null) return;

            try
            {
                _executeLogging(new LogMessage(LogMessage.LogTypeEnum.EXCEPTION,
                                               "Message : " + exception.Message ?? "-",
                                               "InnerMessage : " + (exception.InnerException?.Message ?? "-") +
                                               " , " +
                                               "StackTrace : " + (exception.StackTrace ?? "-"),
                                               "Source : " + (exception.Source ?? "-")));
            }
            catch (Exception ex)
            {
                InnerException.InnerException.LogInnerException(ex);
            }
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


        #endregion



        #region DisposeMethods

        private bool disposed = false;

        private void Dispose(bool disposing)
        {
            if (disposed) return;

            if (disposing)
            {
                try
                {
                    ClearLoggingChannel();
                    _loggingChannels = null;
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
            foreach (ILogger _logger in _loggingChannels)
            {
                try
                {
                    _logger.SaveLog(LogMessage);
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
