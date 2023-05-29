/*---------------------------------------------------------------------------------------------

                ► FastLog.Net , High Performance Logger For .Net ◄



 → Copyright (c) 2020-2023 Shayan Firoozi , Bandar Abbas , Iran , Under MIT License.

 → Contact : <shayan.firoozi@gmail.com>

 → GitHub repository : https://github.com/ShayanFiroozi/FastLog.Net

---------------------------------------------------------------------------------------------*/

using FastLog.Core;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Threading.Tasks;

namespace FastLog.NetTest
{
    internal static class LoggerTest
    {

        private static string LogFile = Path.Combine(AppContext.BaseDirectory, "Logs\\FastLog_LoggerLogs.log");
        private const short MaxLogFileSizeMB = 20;
        private const int TotalTask = 10_000;



        public static readonly Logger FastLogger =
                               Logger.Create().WithInternalLogger(InternalLoggerTest.InternalLoggerAgent)
                                                                  .WithConfiguration(ConfigManager.Create().WithLoggerName("FastLog.Net Crazy Test !"))
                                                                  .WithAgents()
                                                                    .AddTextFileAgent()
                                                                    .UseJsonFormat()
                                                                    .SaveLogToFile(LogFile)
                                                                    .DeleteTheLogFileWhenExceededTheMaximumSizeOf(MaxLogFileSizeMB)
                                                                    .BuildAgent()
                                     .BuildLogger();




        public static async Task CrazyTestWithMultiTasks()
        {
            FastLogger.StartLogger();

            Console.Write($"FastLog.Net Logger test has been started with {TotalTask:N0} tasks simultaneously...\n");

            Stopwatch stopwatch = new Stopwatch();
            stopwatch.Start();


            for (int i = 0; i < TotalTask; i++)
            {
                // Put the log event on queue. ( IMPORTANT : The requests won't process here ! Just put them on queue.)
                await FastLogger.LogException(new InvalidCastException());
            }


            // IMPORTANT : Since the FastLog.Net uses the background engine to process the requested log event(s) ,
            // so we have to wait until all requests in the queue be processes before the app being terminated.

            
            // Until the queue is not empty
            while (FastLogger.ChannelEventCount > 0) 
            { 
                await Task.Yield(); // Wait until all logs in the queue been processed.
            }

   

            stopwatch.Stop();

            Console.Write($"FastLog.Net Logger test has been finished in ");
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.Write($"{stopwatch.ElapsedMilliseconds:N0} Millisecond(s).\n\n");
            Console.ResetColor();

            Console.ForegroundColor = ConsoleColor.Green;
            Console.Write($"Log File : ");
            Console.ResetColor();
            Console.WriteLine($"{LogFile}\n");

            FastLogger.StopLogger();


        }

    }
}
