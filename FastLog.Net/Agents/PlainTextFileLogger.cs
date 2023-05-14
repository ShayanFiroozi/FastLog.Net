using FastLog.Net.Helpers;
using System;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using TrendSoft.FastLog.Interfaces;
using TrendSoft.FastLog.InternalException;
using TrendSoft.FastLog.Models;

namespace TrendSoft.FastLog.Agents
{
    public class PlainTextFileLogger : ILoggerAgent
    {

        private readonly InternalExceptionLogger InternalLogger = null;

        #region Properties

        public string LogFile { get; private set; } = string.Empty;
        private short MaxLogFileSizeMB { get; set; } = 0;

        #endregion


        //Keep it private to make it non accessible from the outside of the class !!
        private PlainTextFileLogger(InternalExceptionLogger internalLogger = null) => InternalLogger = internalLogger;


        public static PlainTextFileLogger Create(InternalExceptionLogger internalLogger = null) => new PlainTextFileLogger(internalLogger);


        public PlainTextFileLogger SaveLogToFile(string filename)
        {
            if (string.IsNullOrWhiteSpace(filename))
            {
                throw new ArgumentException($"'{nameof(filename)}' cannot be null or whitespace.", nameof(filename));
            }


            LogFile = filename;


            if (!Directory.Exists(Path.GetDirectoryName(LogFile)))
            {
                _ = Directory.CreateDirectory(Path.GetDirectoryName(LogFile));
            }


            return this;


        }

        public PlainTextFileLogger NotBiggerThan(short logFileMaxSize)
        {

            if (logFileMaxSize <= 0)
            {
                throw new ArgumentException($"'{nameof(logFileMaxSize)}' must be greater then zero.", nameof(logFileMaxSize));
            }

            MaxLogFileSizeMB = logFileMaxSize;

            return this;

        }


        public Task LogEvent(LogEventModel LogModel, CancellationToken cancellationToken = default)
        {

            if (LogModel is null)
            {
                return Task.CompletedTask;
            }

            try
            {

                CheckAndDeleteLogFileSize();


                return Task.Run(() => ThreadSafeFileHelper.AppendAllText(LogFile, LogModel.GetLogMessage(true)), cancellationToken);

//#if NETFRAMEWORK || NETSTANDARD2_0
//            // May be not "Thread-Safe"
//                //return Task.Run(() => File.AppendAllText(LogFile, LogModel.GetLogMessage(true)), cancellationToken);
//#else
//                // May be not "Thread-Safe"
//                //return File.AppendAllTextAsync(LogFile, LogModel.GetLogMessage(true), cancellationToken);

//#endif



            }
            catch (Exception ex)
            {
                InternalLogger?.LogInternalException(ex);
            }

            return Task.CompletedTask;
        }


        private short GetLogFileSizeMB()
        {
            try
            {
                return string.IsNullOrWhiteSpace(LogFile)
                    ? (short)0
                    : !File.Exists(LogFile) ? (short)0 : (short)(new FileInfo(LogFile).Length / 1024 / 1024);
            }
            catch(Exception ex)
            {
                InternalLogger?.LogInternalException(ex);
                return 0;
            }
        }

        private void CheckAndDeleteLogFileSize()
        {
            try
            {
                if (string.IsNullOrWhiteSpace(LogFile))
                {
                    return;
                }

                if (!File.Exists(LogFile))
                {
                    return;
                }

                if (GetLogFileSizeMB() >= MaxLogFileSizeMB)
                {
                    // May be not "Thread-Safe"
                       //File.Delete(LogFile);

                    ThreadSafeFileHelper.DeleteFile(LogFile);

                }
            }
            catch (Exception ex)
            {
                InternalLogger?.LogInternalException(ex);
            }

        }



    }

}

