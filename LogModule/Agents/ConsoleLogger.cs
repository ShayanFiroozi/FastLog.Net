using System;
using System.IO;
using System.Threading.Channels;
using System.Threading.Tasks;
using TrendSoft.LogModule.Interfaces;
using TrendSoft.LogModule.InternalException;
using TrendSoft.LogModule.Models;

namespace TrendSoft.LogModule.Agents
{
    public class ConsoleLogger : ILoggerAgent
    {


        public Task LogEvent(LogMessageModel LogModel)
        {
            if(LogModel is null) return Task.CompletedTask;

            Console.ForegroundColor = ConsoleColor.Green;
            Console.Write($"{DateTime.Now}");
            Console.ForegroundColor = ConsoleColor.White;

            // Set the proper console forecolor
            switch (LogModel.LogType)
            {
                case LogMessageModel.LogTypeEnum.INFO:
                    Console.ForegroundColor = ConsoleColor.White;
                    break;
                case LogMessageModel.LogTypeEnum.WARNING:
                    Console.ForegroundColor = ConsoleColor.Yellow;
                    break;
                case LogMessageModel.LogTypeEnum.ERROR:
                    Console.ForegroundColor = ConsoleColor.Red;
                    break;
                case LogMessageModel.LogTypeEnum.EXCEPTION:
                    Console.ForegroundColor = ConsoleColor.Red;
                    break;
                case LogMessageModel.LogTypeEnum.DEBUG:
                    Console.ForegroundColor = ConsoleColor.Gray;
                    break;
                default:
                    Console.ForegroundColor = ConsoleColor.White;
                    break;
            }

            Console.WriteLine(LogModel.GetLogMessage());
            Console.WriteLine();


            return Task.CompletedTask;


        }




    }

}

