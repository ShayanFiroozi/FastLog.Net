﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using TrendSoft.FastLog.Agents;
using TrendSoft.FastLog.Core;
using TrendSoft.FastLog.Internal;

namespace FastLog.NetTest
{
    internal static class LoggerTest
    {
        private static Logger loggerA;
        private static Logger loggerB;

        public static void StartLoggers()
        {

            InternalLogger internalLogger = InternalLogger.Create()
                                                   .SaveInternalEventsToFile("D:\\Logs\\InternalEventsLog.LOG")
                                                   .DeleteTheLogFileIfExceededTheMaximumSizeOf(10)
                                                   .Beep()
                                                   .PrintOnConsole()
                                                   .PrintOnDebugWindow();

            loggerA = Logger.Create(internalLogger)


                            //  .WithBeep(BeepAgent.Create())

                            //   .WithPrintOnConsole(ConsoleLogger.Create())

                            //  .WithPrintOnDebugWindow(DebugWindowLogger.Create())

                            .AddPlaintTextFileLogger(PlainTextFileLogger.Create()
                                                                        .SaveLogToFile("D:\\Logs\\TestLogA.log")
                                                                        .DeleteTheLogFileIfExceededTheMaximumSizeOf(5)
                                                                        .IncludeAllEventTypes())


                             .LogMachineName()
                             .LogApplicationName("Shayan-Test-AppA")
                             .RunAgentsInParallel();








            loggerB = Logger.Create(internalLogger)

                     // .WithBeep(BeepAgent.Create())

                     //   .WithPrintOnConsole(ConsoleLogger.Create())

                     //   .WithPrintOnDebugWindow(DebugWindowLogger.Create())

                     .AddPlaintTextFileLogger(PlainTextFileLogger.Create()
                                                                 .SaveLogToFile("D:\\Logs\\TestLogB.log")
                                                                 .DeleteTheLogFileIfExceededTheMaximumSizeOf(5)
                                                                 .IncludeAllEventTypes())

                      .LogMachineName()
                      .LogApplicationName("Shayan-Test-AppB")
                      .RunAgentsInParallel();

            loggerA.StartLogger();
            loggerB.StartLogger();

        }




        public static void CrazyTestWithMultiThreadMultiTask()
        {


            Parallel.For(0, 20, async (y) =>
            {
                _ = loggerA.LogException(new InvalidCastException());
                _ = loggerB.LogException(new InvalidOperationException());
                _ = loggerA.LogException(new DivideByZeroException());
                _ = loggerB.LogException(new FileNotFoundException());



                List<Task> taskList = new List<Task>()
                    {


                        Task.Run(()=>
                      {

                          for (int i = 0; i < 1_000; i++)
                          {
                              _= loggerA.LogInfo($"This is the \"INFO\" message number {i:N0} from the \"LoggerWriteTest\"");
                              _= loggerB.LogAlert($"This is the \"ALERT\" message number {i:N0} from the \"LoggerWriteTest\"");
                              _= loggerA.LogSystem($"This is the \"SYSTEM\" message number {i:N0} from the \"LoggerWriteTest\"");
                              _= loggerB.LogSystem($"This is the \"EXCEPTION\" message number {i:N0} from the \"LoggerWriteTest\"");
                          }


                      }),


                        Task.Run(() =>
                        {

                            for (int j = 0; j < 1_000; j++)
                            {
                                _= loggerA.LogException(new Exception($"This is a \"Test Exception\" number {j:N0} from \"LoggerWriteTest\""));
                                _= loggerB.LogException(new Exception($"This is a \"Test Exception\" number {j:N0} from \"LoggerWriteTest\""));
                                _= loggerA.LogSecurity($"This is a \"Test Security\" number {j:N0} from \"LoggerWriteTest\"");
                                _= loggerB.LogSystem($"This is a \"Test SYSTEM\" number {j:N0} from \"LoggerWriteTest\"");
                                _= loggerA.LogSecurity($"This is a \"Test Security\" number {j:N0} from \"LoggerWriteTest\"");
                                _= loggerB.LogSystem($"This is a \"Test SYSTEM\" number {j:N0} from \"LoggerWriteTest\"");

                            }
                        }),



                        Task.Run(() =>
                        {

                            for (int z = 0; z < 1_000; z++)
                            {
                               _= loggerA.LogError($"This is the \"ERROR\" message number {z:N0} from the \"LoggerWriteTest\"");
                               _= loggerB.LogDebug($"This is the \"DEBUG\" message number {z:N0} from the \"LoggerWriteTest\"");

                            }
                        })


                    };


                await Task.WhenAll(taskList);

            });



        }




    }
}
