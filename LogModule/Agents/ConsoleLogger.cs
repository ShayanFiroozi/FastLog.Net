using System;
using System.IO;
using System.Threading.Channels;
using System.Threading.Tasks;
using TrendSoft.LogModule.Interfaces;
using TrendSoft.LogModule.InternalException;
using TrendSoft.LogModule.Models;

namespace TrendSoft.LogModule.Agents
{

    /// <summary>
    /// WARNING : "Console.WriteLine" has serious performance and memory issues , Be careful when use it !
    /// </summary>
    public class ConsoleLogger : ILoggerAgent
    {

        public Task LogEvent(LogEventModel LogModel)
        {
            if(LogModel is null) return Task.CompletedTask;

            Console.ForegroundColor = ConsoleColor.Green;
            Console.Write($"{DateTime.Now}");
            Console.ForegroundColor = ConsoleColor.White;

            // Set the proper console forecolor
            switch (LogModel.LogType)
            {
                case LogEventModel.LogTypeEnum.INFO:
                    Console.ForegroundColor = ConsoleColor.White;
                    break;
                case LogEventModel.LogTypeEnum.WARNING:
                    Console.ForegroundColor = ConsoleColor.Yellow;
                    break;
                case LogEventModel.LogTypeEnum.ERROR:
                    Console.ForegroundColor = ConsoleColor.Red;
                    break;
                case LogEventModel.LogTypeEnum.EXCEPTION:
                    Console.ForegroundColor = ConsoleColor.Red;
                    break;
                case LogEventModel.LogTypeEnum.DEBUG:
                    Console.ForegroundColor = ConsoleColor.Gray;
                    break;
                default:
                    Console.ForegroundColor = ConsoleColor.White;
                    break;
            }

            Console.WriteLine(LogModel.ToString());
            Console.WriteLine();


            return Task.CompletedTask;


        }




    }

}

