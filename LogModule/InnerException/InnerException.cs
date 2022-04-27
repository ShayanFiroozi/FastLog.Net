using System;
using System.IO;

namespace LogModule.InnerException
{
    public static class InnerException
    {


        /// <summary>
        /// Log the inner exception of Logging module itself !
        /// </summary>
        /// <param name="exception"></param>
        public static void LogInnerException(Exception exception)
        {

            if (exception == null)
            {
                return;
            }

            try
            {


                File.AppendAllText("LoggerLog.log", new LogMessage(LogMessage.LogTypeEnum.EXCEPTION,
                                               "Message : " + exception.Message ?? "-",
                                               "InnerMessage : " + (exception.InnerException?.Message ?? "-") +
                                               " , " +
                                               "StackTrace : " + (exception.StackTrace ?? "-"),
                                               "Source : " + (exception.Source ?? "-")).GetLogMessage().ToString() + Environment.NewLine);

            }
            catch
            {

            }

        }

    }
}
