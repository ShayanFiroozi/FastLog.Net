using System;
using System.IO;
using TrendSoft.FastLog.Models;

namespace TrendSoft.FastLog.InternalException
{
    public static class InternalExceptionLogger
    {


        #region Properties
        private static string InternalExceptionsLogFile { get; set; } = string.Empty;
        private static short InternalExceptionsMaxLogFileSizeMB { get; set; } = 0;

        #endregion



        public static void SetLogFile(string internalExceptionsLogFile)
        {
            if (string.IsNullOrWhiteSpace(internalExceptionsLogFile))
            {
                throw new ArgumentException($"'{nameof(internalExceptionsLogFile)}' cannot be null or whitespace.", nameof(internalExceptionsLogFile));
            }


            InternalExceptionsLogFile = internalExceptionsLogFile;


            if (!Directory.Exists(Path.GetDirectoryName(internalExceptionsLogFile)))
            {
                _ = Directory.CreateDirectory(internalExceptionsLogFile);
            }


        }

        public static void SetLogFileMaxSizeMB(short internalExceptionsMaxLogFileMB)
        {

            if (internalExceptionsMaxLogFileMB <= 0)
            {
                throw new ArgumentException($"'{nameof(internalExceptionsMaxLogFileMB)}' must be greater then zero.", nameof(internalExceptionsMaxLogFileMB));
            }

            InternalExceptionsMaxLogFileSizeMB = internalExceptionsMaxLogFileMB;


        }


        public static void LogInternalException(Exception exception)
        {
            if (string.IsNullOrWhiteSpace(InternalExceptionsLogFile))
            {
                throw new ArgumentException($"'{nameof(InternalExceptionsLogFile)}' cannot be null or whitespace.", nameof(InternalExceptionsLogFile));
            }

            if (InternalExceptionsMaxLogFileSizeMB <= 0)
            {
                throw new ArgumentException($"'{nameof(InternalExceptionsMaxLogFileSizeMB)}' must be greater then zero.", nameof(InternalExceptionsMaxLogFileSizeMB));
            }



            /* Unmerged change from project 'LogModule (net6.0)'
            Before:
                        if (exception is null) return;


                        try
            After:
                        if (exception is null) return;
                        }

                        try
            */
            if (exception is null)
            {
                return;
            }

            try
            {

                try
                {
                    CheckInternalExceptionsLogFileSize();
                }
                catch { }

                LogEventModel LogToSave = new LogEventModel(LogEventModel.LogEventTypeEnum.EXCEPTION,
                                                            " message : " + exception.Message ?? "-",
                                                            " innermessage : " + (exception.InnerException?.Message ?? "-") +
                                                            " , " +
                                                            " stacktrace : " + (exception.StackTrace ?? "-"),
                                                             exception.Source ?? "-");


                File.AppendAllText(InternalExceptionsLogFile, $"{LogToSave.GetLogMessage(true)}\n");

            }
            catch
            {
                return;
            }

            finally
            {
            }

        }




        private static short GetLogFileSizeMB()
        {
            try
            {
                return string.IsNullOrWhiteSpace(InternalExceptionsLogFile) ? (short)0 : !File.Exists(InternalExceptionsLogFile) ? (short)0 :
                (short)(new FileInfo(InternalExceptionsLogFile).Length / 1024 / 1024);
            }
            catch
            {
                return 0;
            }
        }

        private static void CheckInternalExceptionsLogFileSize()
        {
            try
            {
                if (string.IsNullOrWhiteSpace(InternalExceptionsLogFile))
                {
                    return;
                }

                if (!File.Exists(InternalExceptionsLogFile))
                {
                    return;
                }

                if (GetLogFileSizeMB() >= InternalExceptionsMaxLogFileSizeMB)
                {
                    File.Delete(InternalExceptionsLogFile);
                }
            }
            catch
            {

            }

        }





    }

}

