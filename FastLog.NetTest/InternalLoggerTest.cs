using FastLog.Internal;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace FastLog.NetTest
{
    internal static class InternalLoggerTest
    {


        public static readonly InternalLogger InternalLoggerAgent = 
                                                  InternalLogger.Create()
                                                                .SaveInternalEventsToFile("Logs\\InternalLogger.LOG")
                                                                .DeleteTheLogFileWhenExceededTheMaximumSizeOf(20);


        public static async Task CrazyTestMultiThreadWithSameLogFile()
        {
            Console.Write($"Internal Logger test has been started with 1,000 tasks.");
            Console.ForegroundColor = ConsoleColor.Green;
            Console.Write($" {"\"Logs\\InternalLogger.LOG"}...\"\n");
            Console.ResetColor();

            List<Task> taskList = new List<Task>();

            // Create 1_000 simultaneously logging request to the same log file.
            for (int i = 0; i < 1_000; i++)
            {
                taskList.Add(Task.Run(() => InternalLoggerAgent.LogInternalException(new InvalidCastException())));
            }

            await Task.WhenAll(taskList);

            Console.WriteLine($"Interal Logger test has been finished.");

        }

    }
}
