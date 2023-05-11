using System;
using System.Threading;
using System.Threading.Tasks;
using TrendSoft.LogModule.Interfaces;
using TrendSoft.LogModule.Models;

namespace TrendSoft.LogModule.Agents
{

    public class ConsoleLogger : ILoggerAgent
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
            Console.ForegroundColor = LogModel.LogType switch
            {
                LogEventModel.LogTypeEnum.INFO => ConsoleColor.White,
                LogEventModel.LogTypeEnum.WARNING => ConsoleColor.Yellow,
                LogEventModel.LogTypeEnum.ERROR => ConsoleColor.Red,
                LogEventModel.LogTypeEnum.EXCEPTION => ConsoleColor.Red,
                LogEventModel.LogTypeEnum.DEBUG => ConsoleColor.DarkGray,
                _ => ConsoleColor.White,
            };
            Console.WriteLine(LogModel.GetLogMessage(false));
            // Console.WriteLine();


            return Task.CompletedTask;


        }




    }

}

