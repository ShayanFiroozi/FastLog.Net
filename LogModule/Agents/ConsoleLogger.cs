using System;
using System.Threading;
using System.Threading.Tasks;
using TrendSoft.LogModule.Interfaces;
using TrendSoft.LogModule.Models;

namespace TrendSoft.LogModule.Agents
{

    /// <summary>
    /// WARNING : "Console.WriteLine" has serious performance and memory issues , Be careful when use it !
    /// </summary>
    public class ConsoleLogger : ILoggerAgent
    {
        public ConsoleLogger()
        {
#warning "Console.WriteLine" has serious performance and memory issues , Be careful when use it !
        }

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
                LogEventModel.LogTypeEnum.WARNING
 /* Unmerged change from project 'LogModule (net6.0)'
 Before:
                     Console.ForegroundColor = ConsoleColor.Yellow;
                     break;
                 case LogEventModel.LogTypeEnum.ERROR:
 After:
                     Console.ForegroundColor = ConsoleColor.Yellow,
                 case LogEventModel.LogTypeEnum.ERROR:
 */
 => ConsoleColor.Yellow,
                LogEventModel.LogTypeEnum.ERROR => ConsoleColor.Red,
                LogEventModel.LogTypeEnum.EXCEPTION => ConsoleColor.Red,
                LogEventModel.LogTypeEnum.DEBUG => ConsoleColor.Gray,
                _ => ConsoleColor.White,
            };
            Console.WriteLine(LogModel.ToString());
            Console.WriteLine();


            return Task.CompletedTask;


        }




    }

}

