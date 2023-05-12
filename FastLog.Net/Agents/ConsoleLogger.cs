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

        private readonly List<LogEventTypes> _registeredEvents = new List<LogEventTypes>();

        public IEnumerable<LogEventTypes> RegisteredEvents => _registeredEvents;




        private ConsoleLogger()
        {
            //Keep it private to make it non accessible from the outside of the class !!


            RegisterAllEventsToConsole();
        }

        public static ConsoleLogger Create()

        {

            return new ConsoleLogger();
        }



        public ConsoleLogger RegisterEventToConsole(LogEventTypes logEventType)
        {
            if (!_registeredEvents.Any(type => type == logEventType))
            {
                _registeredEvents.Add(logEventType);
            }

            return this;
        }

        public ConsoleLogger UnRegisterEventFromConsole(LogEventTypes logEventType)
        {
            if (_registeredEvents.Any(type => type == logEventType))
            {
                _registeredEvents.Remove(logEventType);
            }

            return this;
        }

        public ConsoleLogger RegisterAllEventsToConsole()
        {
            _registeredEvents.Clear();

            foreach (LogEventTypes eventType in Enum.GetValues(typeof(LogEventTypes)))
            {
                _registeredEvents.Add(eventType);
            }

            return this;
        }

        public ConsoleLogger UnRegisterAllEventsFromConsole()
        {
            _registeredEvents.Clear();

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
            if (!_registeredEvents.Any()) return Task.CompletedTask;


            // Check if current log "Event Type" should be reflected onthe Console or not.
            if (!_registeredEvents.Any(type => LogModel.LogEventType == type)) return Task.CompletedTask;


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

