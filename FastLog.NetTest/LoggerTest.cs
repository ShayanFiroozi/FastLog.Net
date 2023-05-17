﻿using FastLog.Net.Enums;
using System;
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
        public static Logger loggerA;


        public static void StartLoggers()
        {

            InternalLogger internalLogger = InternalLogger.Create()
                                                   .SaveInternalEventsToFile("D:\\Logs\\InternalEventsLog.LOG")
                                                   .DeleteTheLogFileWhenExceededTheMaximumSizeOf(100)
                                                   .Beep()
                                                     .BeepOnlyOnDebugMode()
                                                   .PrintOnConsole()
                                                     .PrintOnConsoleOnlyOnDebugMode()
                                                   .PrintOnDebugWindow();



            loggerA = Logger.Create(internalLogger)


                            //.WithBeep(BeepAgent.Create()
                            //                   .BeepOnlyOnDebugMode())

                            //.WithPrintOnConsole(ConsoleLogger.Create(internalLogger)
                            //                                  .PrintOnConsoleOnlyOnDebugMode()
                            //                                  .ExcludeAllEventTypes()
                            //                                  .IncludeEventType(LogEventTypes.INFO))


                            //     .WithPrintOnDebugWindow(DebugWindowLogger.Create(internalLogger)

                            //.AddHeavyOperationSimulator(HeavyOperationSimulator.Create(TimeSpan.FromSeconds(5)))
                            .AddPlaintTextFileLoggerAgent(PlainTextFileAgent.Create(internalLogger)
                                                                        .SaveLogToFile("D:\\Logs\\TestLog.log")
                                                                        .DeleteTheLogFileWhenExceededTheMaximumSizeOf(1000))


                             .LogMachineName()
                             .LogApplicationName("Shayan-Test-AppA");
            //.RunAgentsInParallel();

            loggerA.StartLogger();


        }




        public static void CrazyTestWithMultiThreadMultiTask()
        {

            //while (true)
            //{

                //Task.Delay(1_500).GetAwaiter().GetResult();

                Parallel.For(0, 10_000, (y) =>
                {
                    _ = loggerA.LogException(new InvalidCastException());
                    _ = loggerA.LogException(new InvalidOperationException());
                    _ = loggerA.LogException(new DivideByZeroException());
                    _ = loggerA.LogException(new FileNotFoundException());

                    _ = loggerA.LogInfo($"This is the \"INFO\" a message from the \"LoggerWriteTest\"");
                    _ = loggerA.LogAlert($"This is the \"ALERT\" a message from the \"LoggerWriteTest\"");
                    _ = loggerA.LogSystem($"This is the \"SYSTEM\" a message number the \"LoggerWriteTest\"");
                    _ = loggerA.LogSystem($"This is the \"EXCEPTION\" a message from the \"LoggerWriteTest\"");




                    List<Task> taskList = new List<Task>()
                    {


                        Task.Run(()=>
                      {


                              _= loggerA.LogInfo($"This is the \"INFO\" message number  from the \"LoggerWriteTest\"");
                              _= loggerA.LogAlert($"This is the \"ALERT\" message number  from the \"LoggerWriteTest\"");
                              _= loggerA.LogSystem($"This is the \"SYSTEM\" message number  from the \"LoggerWriteTest\"");
                              _= loggerA.LogSystem($"This is the \"EXCEPTION\" message number from the \"LoggerWriteTest\"");



                      }),


                        Task.Run(() =>
                        {


                                _= loggerA.LogException(new Exception($"This is a \"Test Exception\" from \"LoggerWriteTest\""));
                                _= loggerA.LogException(new Exception($"This is a \"Test Exception\" from \"LoggerWriteTest\""));
                                _= loggerA.LogSecurity($"This is a \"Test Security\" number  from \"LoggerWriteTest\"");
                                _= loggerA.LogSystem($"This is a \"Test SYSTEM\" number  from \"LoggerWriteTest\"");
                                _= loggerA.LogSecurity($"This is a \"Test Security\"  from \"LoggerWriteTest\"");
                                _= loggerA.LogSystem($"This is a \"Test SYSTEM\"  from \"LoggerWriteTest\"");


                        }),



                        Task.Run(() =>
                        {


                               _= loggerA.LogError($"This is the \"ERROR\" message from the \"LoggerWriteTest\"");
                               _= loggerA.LogDebug($"This is the \"DEBUG\" message from the \"LoggerWriteTest\"");
                               _= loggerA.LogInfo($"This is the \"INFO\" message from the \"LoggerWriteTest\"");
                               _= loggerA.LogNote($"This is the \"NOTE\" message from the \"LoggerWriteTest\"");


                        }),


                   Task.Run(() =>
                        {


                               _= loggerA.LogError($"This is the \"ERROR\" message from the \"LoggerWriteTest\"");
                               _= loggerA.LogDebug($"This is the \"DEBUG\" message from the \"LoggerWriteTest\"");
                               _= loggerA.LogInfo($"This is the \"INFO\" message from the \"LoggerWriteTest\"");
                               _= loggerA.LogNote($"This is the \"NOTE\" message from the \"LoggerWriteTest\"");

                        }),

                      Task.Run(() =>
                        {


                               _= loggerA.LogError($"This is the \"ERROR\" message from the \"LoggerWriteTest\"");
                               _= loggerA.LogDebug($"This is the \"DEBUG\" message from the \"LoggerWriteTest\"");
                               _= loggerA.LogInfo($"This is the \"INFO\" message from the \"LoggerWriteTest\"");
                               _= loggerA.LogNote($"This is the \"NOTE\" message from the \"LoggerWriteTest\"");


                        })

                    };


                    // await Task.WhenAll(taskList);

                });


           // }


        }




    }
}
