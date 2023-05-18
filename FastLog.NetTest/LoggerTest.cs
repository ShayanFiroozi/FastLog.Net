using FastLog.Net.Core;
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
                            .ApplyAgents(AgentsManager.Create()
                                                      //.AddBeepAgent(BeepAgent.Create(internalLogger))
                                                      .AddConsoleAgent(ConsoleAgent.Create(internalLogger))

                                                      .AddTextFileAgent(TextFileAgent.Create(internalLogger)
                                                                                     .UseJsonFormat() 
                                                                                     .SaveLogToFile("D:\\Logs\\TestLog.json")
                                                                                     .DeleteTheLogFileWhenExceededTheMaximumSizeOf(1000))

                                                      .AddTextFileAgent(TextFileAgent.Create(internalLogger)
                                                                                                .SaveLogToFile("D:\\Logs\\TestLog.log")
                                                                                                .DeleteTheLogFileWhenExceededTheMaximumSizeOf(1000))

                                                      .AddMethodExecutionAgent(MethodExecutionAgent.Create(internalLogger).MethodToExecute(MethodA).ExecuteOnlyOnReleaseMode())
                                                      .AddMethodExecutionAgent(MethodExecutionAgent.Create(internalLogger).MethodToExecute(MethodB).ExecuteOnlyOnReleaseMode()))


                             .ApplyConfig(ConfigManager.Create()
                                                       .IncludeMachineName()
                                                       .IncludeApplicationName("Shayan-TestApp"));


            loggerA.StartLogger();
            


        }

        static void MethodA()
        {
            Console.WriteLine("MethodA");

        }

        static void MethodB()
        {
            Console.WriteLine("MethodB");
            Task.Delay(2_000).GetAwaiter().GetResult();
        }



        public static void CrazyTestWithMultiThreadMultiTask()
        {

            //while (true)
            //{

            //Task.Delay(1_500).GetAwaiter().GetResult();

            Parallel.For(0, 10_000, (y) =>
            {
                _ = loggerA.LogException(new InvalidCastException(), 1364);
                _ = loggerA.LogException(new InvalidOperationException(), 1365);
                _ = loggerA.LogException(new DivideByZeroException(), 0);
                _ = loggerA.LogException(new FileNotFoundException(), -1);

                _ = loggerA.LogInfo($"This is the \"INFO\" a message from the \"LoggerWriteTest\"");
                _ = loggerA.LogAlert($"This is the \"ALERT\" a message from the \"LoggerWriteTest\"");
                _ = loggerA.LogSystem($"This is the \"SYSTEM\" a message number the \"LoggerWriteTest\"");
                _ = loggerA.LogSystem($"This is the \"EXCEPTION\" a message from the \"LoggerWriteTest\"");




                List<Task> taskList = new List<Task>()
                {


                        Task.Run(()=>
                      {


                              _= loggerA.LogInfo($"This is the \"INFO\" message number  from the \"LoggerWriteTest\"",EventId:1);
                              _= loggerA.LogAlert($"This is the \"ALERT\" message number  from the \"LoggerWriteTest\"","ANPR",2);
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



        public static void NormalTest()
        {
            while (true)
            {

                Exception exception = new InvalidCastException("ANPR engine cast is not valid",
                                        new CannotUnloadAppDomainException("Engine is not loaded"));


                _ = loggerA.LogException(exception, 1364);

                Task.Delay(2_000).GetAwaiter().GetResult();

                _ = loggerA.LogInfo($"This is an \"INFO\" message from the \"LoggerWriteTest\"");
                Task.Delay(2_000).GetAwaiter().GetResult();

                _ = loggerA.LogAlert($"This is an \"ALERT\" message from the \"LoggerWriteTest\"");
                Task.Delay(2_000).GetAwaiter().GetResult();

                _ = loggerA.LogError($"This is an \"ERROR\" message from the \"LoggerWriteTest\"");

                Task.Delay(2_000).GetAwaiter().GetResult();

                _ = loggerA.LogDebug($"This is a \"DEBUG\" message from the \"LoggerWriteTest\"");

                Task.Delay(2_000).GetAwaiter().GetResult();


                _ = loggerA.LogWarning($"This is a \"WARNING\" message from the \"LoggerWriteTest\"");

                Task.Delay(2_000).GetAwaiter().GetResult();


                _ = loggerA.LogSystem($"This is a \"SYSTEM\" message from the \"LoggerWriteTest\"");

                Task.Delay(2_000).GetAwaiter().GetResult();


                _ = loggerA.LogSecurity($"This is a \"SECURITY\" message from the \"LoggerWriteTest\"");

                Task.Delay(2_000).GetAwaiter().GetResult();
            }

        }



    }

}
