using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using TrendSoft.FastLog.Interfaces;
using TrendSoft.FastLog.Models;

namespace TrendSoft.FastLog.Agents
{

    public class ConsoleLogger : ILoggerAgent
    {

        private readonly List<LogEventModel.LogEventTypeEnum> _logEventTypesToReflect = new List<LogEventModel.LogEventTypeEnum>();
        public IEnumerable<LogEventModel.LogEventTypeEnum> LogEventTypesToReflect => _logEventTypesToReflect;
        
        public void RegisterEventTypeToReflect(LogEventModel.LogEventTypeEnum logEventType) 
        {
            if(!_logEventTypesToReflect.Any(type=>type == logEventType))
            {
                _logEventTypesToReflect.Add(logEventType);
            }
        }

        public bool UnregisterEventTypeFromReflecting(LogEventModel.LogEventTypeEnum logEventType)
        {
            if (_logEventTypesToReflect.Any(type => type == logEventType))
            {
               return _logEventTypesToReflect.Remove(logEventType);
            }

            return false;
        }

        public void RegisterAllEventToReflect() => _logEventTypesToReflect.Clear();


        public Task LogEvent(LogEventModel LogModel, CancellationToken cancellationToken = default)
        {
            if (LogModel is null)
            {
                return Task.CompletedTask;
            }

            // Check if current log event type should be reflected onthe Console or not.
            if(_logEventTypesToReflect.Any())
            {
                if (!_logEventTypesToReflect.Any(type => LogModel.LogType == type)) return Task.CompletedTask;
            }

            Console.ForegroundColor = ConsoleColor.Green;
            Console.Write($"{DateTime.Now}");
            Console.ForegroundColor = ConsoleColor.White;


            // Set the proper console forecolor
            switch (LogModel.LogType)
            {
                case LogEventModel.LogEventTypeEnum.INFO:
                    Console.ForegroundColor = ConsoleColor.White;
                    break;
                case LogEventModel.LogEventTypeEnum.WARNING:
                    Console.ForegroundColor = ConsoleColor.Yellow;
                    break;
                case LogEventModel.LogEventTypeEnum.ALERT :
                    Console.ForegroundColor = ConsoleColor.Red;
                    break;
                case LogEventModel.LogEventTypeEnum.ERROR:
                    Console.ForegroundColor = ConsoleColor.DarkRed;
                    break;
                case LogEventModel.LogEventTypeEnum.EXCEPTION:
                    Console.ForegroundColor = ConsoleColor.Red;
                    break;
                case LogEventModel.LogEventTypeEnum.DEBUG:
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

