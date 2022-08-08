using System;
using System.IO;

namespace LogModule.InnerException
{
    public static class InnerException
    {
        public const string InnerExceptionsLogFile = "LoggerExceptions.log";
        private const int LOG_FILE_MAX_SIZE_IN_MB = 100;

        private static int InnerExceptionLogFileSizeMB
        {
            get
            {
                try
                {
                    return Convert.ToInt32((new FileInfo(InnerExceptionsLogFile).Length / 1024) / 1024);
                }
                catch
                {
                    return 0;
                }
            }
        }



        public static void DeleteInnerExceptionLogFile()
        {
            try
            {

                if (!File.Exists(InnerExceptionsLogFile))
                {
                    return;
                }


                if (InnerExceptionLogFileSizeMB
                     <= LOG_FILE_MAX_SIZE_IN_MB)
                {
                    return;
                }
            }
            catch
            {

            }




            try
            {
                File.Delete(InnerExceptionsLogFile);

            }
            catch
            {

            }
        }


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


                File.AppendAllText(InnerExceptionsLogFile, new LogMessage(LogMessage.LogTypeEnum.EXCEPTION,
                                               " Message : " + exception.Message ?? "-",
                                               " InnerMessage : " + (exception.InnerException?.Message ?? "-") +
                                               " , " +
                                               " StackTrace : " + (exception.StackTrace ?? "-"),
                                                (exception.Source ?? "-")).GetLogMessage().ToString() + Environment.NewLine);

            }
            catch
            {

            }

        }


    }
}
