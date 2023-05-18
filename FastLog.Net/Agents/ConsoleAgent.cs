using FastLog.Enums;
using FastLog.Net.Helpers.ExtendedMethods;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using TrendSoft.FastLog.Interfaces;
using TrendSoft.FastLog.Internal;
using TrendSoft.FastLog.Models;

namespace TrendSoft.FastLog.Agents
{

    // Note : ConsoleLogger class uses fluent "Builder" pattern.

    public class ConsoleAgent : ILoggerAgent
    {

        private readonly List<LogEventTypes> _registeredEvents = new List<LogEventTypes>();
        private readonly InternalLogger InternalLogger = null;
        private bool executeOnlyOnDebugMode { get; set; } = false;
        private bool executeOnlyOnReleaseMode { get; set; } = false;

        private bool jsonFormat { get; set; } = false;


        #region Fluent Builder Methods

        //Keep it private to make it non accessible from the outside of the class !!
        private ConsoleAgent(InternalLogger internalLogger)
        {
            InternalLogger = internalLogger;
            IncludeAllEventTypes();
        }


        public static ConsoleAgent Create(InternalLogger internalLogger = null) => new ConsoleAgent(internalLogger);


        public ConsoleAgent ExecuteOnlyOnDebugMode()
        {
            executeOnlyOnDebugMode = true;
            return this;
        }

        public ConsoleAgent ExecuteOnlyOnReleaseMode()
        {
            executeOnlyOnReleaseMode = true;
            return this;
        }

        public ConsoleAgent UseJsonFormat()
        {
            jsonFormat = true;
            return this;
        }

        public ConsoleAgent IncludeEventType(LogEventTypes logEventType)
        {
            if (!_registeredEvents.Any(type => type == logEventType))
            {
                _registeredEvents.Add(logEventType);
            }

            return this;
        }

        public ConsoleAgent ExcludeEventType(LogEventTypes logEventType)
        {
            if (_registeredEvents.Any(type => type == logEventType))
            {
                _registeredEvents.Remove(logEventType);
            }

            return this;
        }

        public ConsoleAgent IncludeAllEventTypes()
        {
            _registeredEvents.Clear();

            foreach (LogEventTypes eventType in Enum.GetValues(typeof(LogEventTypes)))
            {
                _registeredEvents.Add(eventType);
            }

            return this;
        }

        public ConsoleAgent ExcludeAllEventTypes()
        {
            _registeredEvents.Clear();

            return this;
        }


        #endregion


        public Task ExecuteAgent(LogEventModel LogModel, CancellationToken cancellationToken = default)
        {

#if !RELEASE

            if (executeOnlyOnReleaseMode) return Task.CompletedTask;

#endif

#if !DEBUG
            if (executeOnlyOnDebugMode) return Task.CompletedTask;

#endif
            if (LogModel is null)
            {
                return Task.CompletedTask;
            }

            try
            {

                // Check if current log "Event Type" should be execute or not.
                if (!_registeredEvents.Any()) return Task.CompletedTask;
                if (!_registeredEvents.Any(type => LogModel.LogEventType == type)) return Task.CompletedTask;


                // Set the proper console forecolor

                switch (LogModel.LogEventType)
                {
                    case LogEventTypes.INFO:
                    case LogEventTypes.NOTE:
                    case LogEventTypes.TODO:
                        Console.ForegroundColor = ConsoleColor.White;
                        break;

                    case LogEventTypes.WARNING:
                        Console.ForegroundColor = ConsoleColor.Yellow;
                        break;

                    case LogEventTypes.ALERT:
                        Console.ForegroundColor = ConsoleColor.DarkRed;
                        break;

                    case LogEventTypes.DEBUG:
                        Console.ForegroundColor = ConsoleColor.DarkGray;
                        break;

                    case LogEventTypes.ERROR:
                    case LogEventTypes.EXCEPTION:
                        Console.ForegroundColor = ConsoleColor.Red;
                        break;


                    case LogEventTypes.SYSTEM:
                        Console.ForegroundColor = ConsoleColor.Blue;
                        break;

                    case LogEventTypes.SECURITY:
                        Console.ForegroundColor = ConsoleColor.Cyan;
                        break;

                    default:
                        break;
                }


                Console.WriteLine(jsonFormat ? LogModel.ToJsonText() : LogModel.ToPlainText());


            }
            catch (Exception ex)
            {
                InternalLogger?.LogInternalException(ex);
            }

            return Task.CompletedTask;


        }


    }

}

