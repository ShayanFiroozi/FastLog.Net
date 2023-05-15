using FastLog.Net.Enums;
using FastLog.Net.Helpers;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using TrendSoft.FastLog.Interfaces;
using TrendSoft.FastLog.InternalException;
using TrendSoft.FastLog.Models;

namespace TrendSoft.FastLog.Agents
{
    // Note : DebugWindowLogger class uses fluent "Builder" pattern.

    public class PlainTextFileLogger : ILoggerAgent
    {
        private readonly List<LogEventTypes> _registeredEvents = new List<LogEventTypes>();
        private readonly InternalExceptionLogger InternalLogger = null;

        #region Properties

        public string LogFile { get; private set; } = string.Empty;
        private short MaxLogFileSizeMB { get; set; } = 0;

        #endregion


        //Keep it private to make it non accessible from the outside of the class !!
        private PlainTextFileLogger(InternalExceptionLogger internalLogger = null) => InternalLogger = internalLogger;


        public static PlainTextFileLogger Create(InternalExceptionLogger internalLogger = null) => new PlainTextFileLogger(internalLogger);


        public PlainTextFileLogger IncludeEventType(LogEventTypes logEventType)
        {
            if (!_registeredEvents.Any(type => type == logEventType))
            {
                _registeredEvents.Add(logEventType);
            }

            return this;
        }

        public PlainTextFileLogger ExcludeEventType(LogEventTypes logEventType)
        {
            if (_registeredEvents.Any(type => type == logEventType))
            {
                _registeredEvents.Remove(logEventType);
            }

            return this;
        }

        public PlainTextFileLogger IncludeAllEventTypes()
        {
            _registeredEvents.Clear();

            foreach (LogEventTypes eventType in Enum.GetValues(typeof(LogEventTypes)))
            {
                _registeredEvents.Add(eventType);
            }

            return this;
        }

        public PlainTextFileLogger ExcludeAllEventTypes()
        {
            _registeredEvents.Clear();

            return this;
        }


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


                // Check if any "Event Type" exists to show on Debug Window ?
                if (!_registeredEvents.Any()) return Task.CompletedTask;


                // Check if current log "Event Type" should be reflected onthe Debug Window or not.
                if (!_registeredEvents.Any(type => LogModel.LogEventType == type)) return Task.CompletedTask;


                CheckAndDeleteLogFileSize();

                //Note :  In code below we will lose exceptions from "ThreadSafeFileHelper.AppendAllText" due to use a "Fire and forget" approach here.
                // Please note that it is not recomended to use "ReaderWriterLockSlim" in async/await methods.
                //For more info please visit -> https://stackoverflow.com/questions/19659387/readerwriterlockslim-and-async-await

                // #Refactor Requied. ( goal : use an approach to be able to catch exceptions properly and not using "Fire and Forget" )
                return Task.Run(() => ThreadSafeFileHelper.AppendAllText(LogFile, LogModel.GetLogMessage(true)), cancellationToken);


                // Note : The approach below (when using File.AppendAllTextAsync) is not thread-safe and has some issues ,
                // - specially when two or more loggers are logging to the same file.

                //#if NETFRAMEWORK || NETSTANDARD2_0

                //                await Task.Run(() => ThreadSafeFileHelper.AppendAllText(LogFile, LogModel.GetLogMessage(true)), cancellationToken);
                //#else

                //                await File.AppendAllTextAsync(LogFile, LogModel.GetLogMessage(true), cancellationToken);

                //#endif



            }
            catch (Exception ex)
            {
                InternalLogger?.LogInternalException(ex);
            }

            return Task.CompletedTask;
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

                if (ThreadSafeFileHelper.GetFileSize(LogFile) >= MaxLogFileSizeMB)
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

