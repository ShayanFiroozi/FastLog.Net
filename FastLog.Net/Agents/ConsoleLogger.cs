﻿using FastLog.Net.Enums;
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
            //Keep it private just make it non accessible from the outside of the class !!


            ReflectAllEventTypeToConsole();
        }

        public static ConsoleLogger Create()

        {

            return new ConsoleLogger();
        }



        public ConsoleLogger ReflectEventTypeToConsole(LogEventTypes logEventType)
        {
            if (!_logEventTypesToReflect.Any(type => type == logEventType))
            {
                _logEventTypesToReflect.Add(logEventType);
            }

            return this;
        }

        public ConsoleLogger DoNotReflectEventTypeToConsole(LogEventTypes logEventType)
        {
            if (_logEventTypesToReflect.Any(type => type == logEventType))
            {
                _logEventTypesToReflect.Remove(logEventType);
            }

            return this;
        }

        public ConsoleLogger ReflectAllEventTypeToConsole()
        {
            _logEventTypesToReflect.Clear();

            foreach (LogEventTypes eventType in Enum.GetValues(typeof(LogEventTypes)))
            {
                _logEventTypesToReflect.Add(eventType);
            }

            return this;
        }

        public ConsoleLogger DoNotReflectAnyEventTypeToConsole()
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
                    Console.ForegroundColor = ConsoleColor.Yellow;
                    break;
                case LogEventTypes.ALERT:
                    Console.ForegroundColor = ConsoleColor.Red;
                    break;
                case LogEventTypes.ERROR:
                    Console.ForegroundColor = ConsoleColor.DarkRed;
                    break;
                case LogEventTypes.EXCEPTION:
                    Console.ForegroundColor = ConsoleColor.Red;
                    break;
                case LogEventTypes.DEBUG:
                    Console.ForegroundColor = ConsoleColor.Gray;
                    break;
                default:
                    Console.ForegroundColor = ConsoleColor.White;
                    break;
            }


            Console.WriteLine(LogModel.GetLogMessage(false));

            Console.ForegroundColor = ConsoleColor.White;


            return Task.CompletedTask;


        }




    }

}

