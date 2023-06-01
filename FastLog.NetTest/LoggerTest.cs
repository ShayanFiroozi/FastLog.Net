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


        // Build FastLog.Net configuration with fluent builder pattern.
        private static ConfigManager fastLoggerConfig = ConfigManager.Create()
                                                                     .WithLoggerName("FastLog.Net Crazy Test !");

        // Build Fast Logger with fluent builder pattern.
        public static readonly Logger FastLogger =
                               Logger.Create().WithInternalLogger(InternalLoggerTest.InternalLoggerAgent)
                                              .WithConfiguration(fastLoggerConfig)
                                                 .WithAgents()
                                                   .AddTextFileAgent()
                                                    //.UseJsonFormat()
                                                    .SaveLogToFile(LogFile)
                                                    .DeleteTheLogFileWhenExceededTheMaximumSizeOf(MaxLogFileSizeMB)
                                                   .BuildAgent()
                                      .BuildLogger();


        static LoggerTest()
        {
            FastLogger.StartLogger();
        }


        public static async Task CrazyTestWithMultiTasks()
        {


            Console.Write($"FastLog.Net Logger test has been started with {TotalTask:N0} tasks simultaneously...\n");

            Stopwatch stopwatch = new Stopwatch();
            stopwatch.Start();

            // (TotalTask / 2) because we will log two events instead of 1

            for (int i = 0; i < (TotalTask / 2); i++)
            {
                // Put the log event on queue. ( IMPORTANT : The requests won't process here ! Just put them on queue.)
                await FastLogger.LogException(new InvalidCastException("This is the Exception i want to throw !!"));
                await FastLogger.LogInfo("FastLog.Net , high performance logger for .Net");

            }


            // IMPORTANT : Since the FastLog.Net uses the background engine to process the requested log event(s) ,
            // so we HAVE TO await "ProcessAllEventsInQueue" method until all requests in the queue be processed before the app termination.

            await FastLogger.ProcessAllEventsInQueue();


            stopwatch.Stop();

            Console.Write($"FastLog.Net Logger test has been finished in ");
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.Write($"{stopwatch.ElapsedMilliseconds:N0} Millisecond(s).\n\n");
            Console.ResetColor();

            Console.ForegroundColor = ConsoleColor.Green;
            Console.Write($"Log File : ");
            Console.ResetColor();
            Console.WriteLine($"{LogFile}\n");




        }



        public static async Task ForEverRunTest()
        {
            while (true)
            {
                // Add 60 log events suddenly !! to test the FastLog.Net peocessing engine and queue management.

                for (int i = 0; i < 20; i++)
                {
                    await FastLogger.LogException(new InvalidCastException());
                    await FastLogger.LogInfo("This is a ForEver run test and won't stop until you stop it manually !!");
                    await FastLogger.LogTodo("Please like the FastLog.Net on GitHub : \"https://github.com/ShayanFiroozi/FastLog.Net\"");
                }

                // Note : If you have a good CPU and also a SSD ! you can decrease the waiting milliseconds ;)
                // WARNING : Watch your memory during the forever test because the memory will increase exponentially for the time of processing events take more time than adding them to the queue.

                await Task.Delay(TimeSpan.FromMilliseconds(50));

                Console.WriteLine($"Total Log Events: {FastLogger.QueueTotalEventCount:N0} , " +
                                  $"Total Processed Log Events: {FastLogger.QueueProcessedEventCount:N0} , " +
                                  $"Remaining Log Events in Queue : {FastLogger.QueueEventCount:N0}");

                if(Console.KeyAvailable) 
                {
                    break;
                }

            }
        }




    }
}
