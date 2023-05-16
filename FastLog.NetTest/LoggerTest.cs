using FastLog.Net.Enums;
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
        private static Logger loggerA;


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
                            .AddPlaintTextFileLogger(PlainTextFileLogger.Create(internalLogger)
                                                                        .SaveLogToFile("D:\\Logs\\TestLog.log")
                                                                        .DeleteTheLogFileWhenExceededTheMaximumSizeOf(10))


                             .LogMachineName()
                             .LogApplicationName("Shayan-Test-AppA");
            //.RunAgentsInParallel();

            loggerA.StartLogger();


        }




        public static void CrazyTestWithMultiThreadMultiTask()
        {


            Parallel.For(0, 100, async (y) =>
            {
                _ = loggerA.LogException(new InvalidCastException());
                _ = loggerA.LogException(new InvalidOperationException());
                _ = loggerA.LogException(new DivideByZeroException());
                _ = loggerA.LogException(new FileNotFoundException());



                List<Task> taskList = new List<Task>()
                    {


                        Task.Run(()=>
                      {

                          for (int i = 0; i < 1_000; i++)
                          {
                              _= loggerA.LogInfo($"This is the \"INFO\" message number {i:N0} from the \"LoggerWriteTest\"");
                              _= loggerA.LogAlert($"This is the \"ALERT\" message number {i:N0} from the \"LoggerWriteTest\"");
                              _= loggerA.LogSystem($"This is the \"SYSTEM\" message number {i:N0} from the \"LoggerWriteTest\"");
                              _= loggerA.LogSystem($"This is the \"EXCEPTION\" message number {i:N0} from the \"LoggerWriteTest\"");
                          }


                      }),


                        Task.Run(() =>
                        {

                            for (int j = 0; j < 1_000; j++)
                            {
                                _= loggerA.LogException(new Exception($"This is a \"Test Exception\" number {j:N0} from \"LoggerWriteTest\""));
                                _= loggerA.LogException(new Exception($"This is a \"Test Exception\" number {j:N0} from \"LoggerWriteTest\""));
                                _= loggerA.LogSecurity($"This is a \"Test Security\" number {j:N0} from \"LoggerWriteTest\"");
                                _= loggerA.LogSystem($"This is a \"Test SYSTEM\" number {j:N0} from \"LoggerWriteTest\"");
                                _= loggerA.LogSecurity($"This is a \"Test Security\" number {j:N0} from \"LoggerWriteTest\"");
                                _= loggerA.LogSystem($"This is a \"Test SYSTEM\" number {j:N0} from \"LoggerWriteTest\"");

                            }
                        }),



                        Task.Run(() =>
                        {

                            for (int z = 0; z < 1_000; z++)
                            {
                               _= loggerA.LogError($"This is the \"ERROR\" message number {z:N0} from the \"LoggerWriteTest\"");
                               _= loggerA.LogDebug($"This is the \"DEBUG\" message number {z:N0} from the \"LoggerWriteTest\"");
                                    _= loggerA.LogException(new Exception($"This is a \"Test Exception\" number {z:N0} from \"LoggerWriteTest\""));
                                _= loggerA.LogException(new Exception($"This is a \"Test Exception\" number {z:N0} from \"LoggerWriteTest\""));
                                _= loggerA.LogSecurity($"This is a \"Test Security\" number {z:N0} from \"LoggerWriteTest\"");
                                _= loggerA.LogSystem($"This is a \"Test SYSTEM\" number {z:N0} from \"LoggerWriteTest\"");
                                _= loggerA.LogSecurity($"This is a \"Test Security\" number {z:N0} from \"LoggerWriteTest\"");
                                _= loggerA.LogSystem($"This is a \"Test SYSTEM\" number {z:N0} from \"LoggerWriteTest\"");

                            }
                        }),


                   Task.Run(() =>
                        {

                            for (int h = 0; h < 1_000; h++)
                            {
                               _= loggerA.LogError($"This is the \"ERROR\" message number {h:N0} from the \"LoggerWriteTest\"");
                               _= loggerA.LogDebug($"This is the \"DEBUG\" message number {h:N0} from the \"LoggerWriteTest\"");
                                    _= loggerA.LogException(new Exception($"This is a \"Test Exception\" number {h:N0} from \"LoggerWriteTest\""));
                                _= loggerA.LogException(new Exception($"This is a \"Test Exception\" number {h:N0} from \"LoggerWriteTest\""));
                                _= loggerA.LogSecurity($"This is a \"Test Security\" number {h:N0} from \"LoggerWriteTest\"");
                                _= loggerA.LogSystem($"This is a \"Test SYSTEM\" number {h:N0} from \"LoggerWriteTest\"");
                                _= loggerA.LogSecurity($"This is a \"Test Security\" number {h:N0} from \"LoggerWriteTest\"");
                                _= loggerA.LogSystem($"This is a \"Test SYSTEM\" number {h:N0} from \"LoggerWriteTest\"");

                            }
                        }),

                      Task.Run(() =>
                        {

                            for (int m = 0; m < 1_000; m++)
                            {
                               _= loggerA.LogError($"This is the \"ERROR\" message number {m:N0} from the \"LoggerWriteTest\"");
                               _= loggerA.LogDebug($"This is the \"DEBUG\" message number {m:N0} from the \"LoggerWriteTest\"");
                                    _= loggerA.LogException(new Exception($"This is a \"Test Exception\" number {m:N0} from \"LoggerWriteTest\""));
                                _= loggerA.LogException(new Exception($"This is a \"Test Exception\" number {m:N0} from \"LoggerWriteTest\""));
                                _= loggerA.LogSecurity($"This is a \"Test Security\" number {m:N0} from \"LoggerWriteTest\"");
                                _= loggerA.LogSystem($"This is a \"Test SYSTEM\" number {m:N0} from \"LoggerWriteTest\"");
                                _= loggerA.LogSecurity($"This is a \"Test Security\" number {m:N0} from \"LoggerWriteTest\"");
                                _= loggerA.LogSystem($"This is a \"Test SYSTEM\" number {m:N0} from \"LoggerWriteTest\"");

                            }
                        })

                    };


             //   await Task.WhenAll(taskList);

            });



        }




    }
}
