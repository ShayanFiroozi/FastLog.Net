using FastLog.Net.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using TrendSoft.FastLog.Interfaces;
using TrendSoft.FastLog.Models;

namespace TrendSoft.FastLog.Agents
{

    // Note : ConsoleLogger class uses fluent "Builder" pattern.

    public class ConsoleLogger : ILoggerAgent
    {

        private ConsoleColor DateTimeFontColor = ConsoleColor.Green;

        private readonly List<LogEventTypes> _logEventTypesToReflect = new List<LogEventTypes>();

        public IEnumerable<LogEventTypes> LogEventTypesToReflect => _logEventTypesToReflect;




        private ConsoleLogger()
        {
            //Keep it private to make it non accessible from the outside of the class !!


            ReflectAllEventsToConsole();
        }

        public static ConsoleLogger Create()

        {

            return new ConsoleLogger();
        }



        public ConsoleLogger ReflectOnConsole(LogEventTypes logEventType)
        {
            if (!_logEventTypesToReflect.Any(type => type == logEventType))
            {
                _logEventTypesToReflect.Add(logEventType);
            }

            return this;
        }

        public ConsoleLogger DoNotReflectOnConsole(LogEventTypes logEventType)
        {
            if (_logEventTypesToReflect.Any(type => type == logEventType))
            {
                _logEventTypesToReflect.Remove(logEventType);
            }

            return this;
        }

        public ConsoleLogger ReflectAllEventsToConsole()
        {
            _logEventTypesToReflect.Clear();

            foreach (LogEventTypes eventType in Enum.GetValues(typeof(LogEventTypes)))
            {
                _logEventTypesToReflect.Add(eventType);
            }

            return this;
        }

        public ConsoleLogger DoNotReflectAllEventsToConsole()
        {
            _logEventTypesToReflect.Clear();

            return this;
        }

        public ConsoleLogger WithDateTimeFontColor(ConsoleColor color)
        {
            DateTimeFontColor = color;
            return this;
        }




        public Task LogEvent(LogEventModel LogModel, CancellationToken cancellationToken = default)
        {
            if (LogModel is null)
            {
                return Task.CompletedTask;
            }

            // Check if any "Event Type" exists to show on console ?
            if (!_logEventTypesToReflect.Any()) return Task.CompletedTask;


            // Check if current log "Event Type" should be reflected onthe Console or not.
            if (!_logEventTypesToReflect.Any(type => LogModel.LogEventType == type)) return Task.CompletedTask;


            Console.ForegroundColor = DateTimeFontColor;
            Console.Write($"{DateTime.Now}");
            Console.ForegroundColor = ConsoleColor.White;


            // Set the proper console forecolor

            switch (LogModel.LogEventType)
            {
                case LogEventTypes.INFO:
                    Console.ForegroundColor = ConsoleColor.White;
                    break;
                case LogEventTypes.WARNING:
                    Console.ForegroundColor = ConsoleColor.DarkYellow;
                    break;
                case LogEventTypes.ALERT:
                    Console.ForegroundColor = ConsoleColor.DarkRed;
                    break;
                case LogEventTypes.DEBUG:
                    Console.ForegroundColor = ConsoleColor.DarkGray;
                    break;
                case LogEventTypes.ERROR:
                    Console.ForegroundColor = ConsoleColor.Red;
                    break;
                case LogEventTypes.EXCEPTION:
                    Console.ForegroundColor = ConsoleColor.Yellow;
                    break;
                case LogEventTypes.SYSTEM:
                    Console.ForegroundColor = ConsoleColor.Magenta;
                    break;
                default:
                    break;
            }


            Console.WriteLine(LogModel.GetLogMessage(false));

            Console.ForegroundColor = ConsoleColor.White;


            return Task.CompletedTask;


        }




    }

}

