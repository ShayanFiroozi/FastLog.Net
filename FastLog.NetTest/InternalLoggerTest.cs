/*---------------------------------------------------------------------------------------------

                ► FastLog.Net , High Performance Logger For .Net ◄



 → Copyright (c) 2020-2023 Shayan Firoozi , Bandar Abbas , Iran , Under MIT License.

 → Contact : <shayan.firoozi@gmail.com>

 → GitHub repository : https://github.com/ShayanFiroozi/FastLog.Net

---------------------------------------------------------------------------------------------*/

using FastLog.Internal;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Threading.Tasks;

namespace FastLog.NetTest
{
    internal static class InternalLoggerTest
    {

        private static string LogFile = Path.Combine(AppContext.BaseDirectory, "Logs\\FastLog_InternalLogs.log");
        private const short MaxLogFileSizeMB = 10;
        private const int TotalTask = 10_000;


        // Build FastLog.Net internal logger with fluent builder pattern.
        public static readonly InternalLogger InternalLoggerAgent =
                                                  InternalLogger.Create()
                                                                .SaveInternalEventsToFile(LogFile)
                                                                .UseJsonFormat()
                                                                .DeleteTheLogFileWhenExceededTheMaximumSizeOf(MaxLogFileSizeMB);


        public static async Task CrazyTestWithMultiTasks()
        {
            Console.Write($"Internal Logger test has been started with {TotalTask:N0} tasks simultaneously...\n");
            
            List<Task> taskList = new List<Task>();

            Stopwatch stopwatch = new Stopwatch();
            stopwatch.Start();

          
            for (int i = 0; i < TotalTask; i++)
            {
                taskList.Add(Task.Run(() => InternalLoggerAgent.LogInternalException(new InvalidCastException())));
            }

            await Task.WhenAll(taskList);

            stopwatch.Stop();

            Console.Write($"Internal Logger test has been finished in ");
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.Write($"{stopwatch.ElapsedMilliseconds:N0} Millisecond(s).\n\n");
            Console.ResetColor();

            Console.ForegroundColor = ConsoleColor.Green;
            Console.Write($"Log File : ");
            Console.ResetColor();
            Console.WriteLine($"{LogFile}\n");


        }

    }
}
