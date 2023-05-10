using System;
using System.IO;
using System.Threading.Channels;
using System.Threading.Tasks;
using TrendSoft.LogModule.Models;

namespace TrendSoft.LogModule.InternalException
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
                Directory.CreateDirectory(internalExceptionsLogFile);
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


            if (exception is null) return;


            try
            {

                try
                {
                    CheckInternalExceptionsLogFileSize();
                }
                catch { }

                using LogEventModel logtosave = new LogEventModel(LogEventModel.LogTypeEnum.EXCEPTION,
                                                            " message : " + exception.Message ?? "-",
                                                            " innermessage : " + (exception.InnerException?.Message ?? "-") +
                                                            " , " +
                                                            " stacktrace : " + (exception.StackTrace ?? "-"),
                                                             (exception.Source ?? "-"));


                File.AppendAllText(InternalExceptionsLogFile, logtosave.GetLogMessage().ToString() + Environment.NewLine);

            }
            catch
            {
                return;
            }

            finally
            {
                exception = null;
            }

        }




        private static short GetLogFileSizeMB()
        {
            try
            {
                if (string.IsNullOrWhiteSpace(InternalExceptionsLogFile)) return (short)0;
                if (!File.Exists(InternalExceptionsLogFile)) return (short)0;

                return (short)((new FileInfo(InternalExceptionsLogFile).Length / 1024) / 1024);
            }
            catch
            {
                return (short)0;
            }
        }

        private static void CheckInternalExceptionsLogFileSize()
        {
            try
            {
                if (string.IsNullOrWhiteSpace(InternalExceptionsLogFile)) return;
                if (!File.Exists(InternalExceptionsLogFile)) return;


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

