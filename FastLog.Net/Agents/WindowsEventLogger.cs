using FastLog.Net.Enums;
using System;
using System.Threading;
using System.Threading.Tasks;
using TrendSoft.FastLog.Interfaces;
using TrendSoft.FastLog.Models;

namespace TrendSoft.FastLog.Agents
{

    public class WindowsEventLogger : ILoggerAgent
    {


        public Task LogEvent(LogEventModel LogModel, CancellationToken cancellationToken = default)
        {
            if (LogModel is null)
            {
                return Task.CompletedTask;
            }

            Console.ForegroundColor = ConsoleColor.Green;
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
                case LogEventTypes.ALERT :
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

