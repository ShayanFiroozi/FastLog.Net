﻿using FastLog.Core;
using FastLog.Internal;
using FastLog.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace FastLog.NetTest
{
    internal static class LoggerTest
    {
        public static Logger loggerA;



        public static async void StartLoggers()
        {

            InternalLogger internalLogger = InternalLogger.Create()
                                                          .UseJsonFormat()
                                                          .SaveInternalEventsToFile("D:\\Logs\\InternalEventsLog.log")
                                                          .DeleteTheLogFileWhenExceededTheMaximumSizeOf(100)
                                                          //.Beep()
                                                          //  .BeepOnlyOnDebugMode()
                                                          .PrintOnConsole();
            // .PrintOnConsoleOnlyOnDebugMode()
            // .PrintOnDebugWindow();




            ConfigManager loggerConfig = ConfigManager.Create()
                                                      .WithLoggerName("ANPR® Logger")
                                                      .WithMaxEventsToKeepInMemory(1_000);



            loggerA = Logger.Create()
                            .WithInternalLogger(internalLogger)
                            .WithConfiguration(loggerConfig)
                            .WithAgents()

                                .AddTextFileAgent()
                                   .UseJsonFormat()
                                   .SaveLogToFile("D:\\Logs\\TestLog.json")
                                   .DeleteTheLogFileWhenExceededTheMaximumSizeOf(2)
                                    .BuildAgent()




            //.AddConsoleAgent().UseJsonFormat().BuildAgent()



            .BuildLogger();

            loggerA.StartLogger();


            // Causing exception !
            //await Task.Run(async () =>
            //    {
            //        while (true)
            //        {
            //            foreach (LogEventModel logEvent in loggerA.InMemoryEvents.ToList())
            //            {

            //                Console.WriteLine(loggerA.InMemoryEvents.Count());
            //                Console.WriteLine(loggerA.InMemoryEvents.First().EventMessage);
            //                Console.WriteLine();

            //            }


            //        }

            //    });



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



        public static async void CrazyTestWithMultiThreadMultiTask()
        {

            //while (true)
            //{

            //Task.Delay(1_500).GetAwaiter().GetResult();

            Parallel.For(0, 40_000, (y) =>
            {
                loggerA.LogException(new InvalidCastException("ANPR engine cast is not valid",
                                            new CannotUnloadAppDomainException("Engine is not loaded",
                                            new AccessViolationException("Just kiddin U !",
                                            new InvalidTimeZoneException("Really  kiddin With U !")))), 1365);

                _ = loggerA.LogException(new InvalidOperationException(), 1365);
                _ = loggerA.LogException(new DivideByZeroException(), 0);
                _ = loggerA.LogException(new FileNotFoundException(), -1);


                _ = loggerA.LogInfo($"This is the \"INFO\" a message from the \"CrazyTestWithMultiThreadMultiTask\"");
                _ = loggerA.LogAlert($"This is the \"ALERT\" a message from the \"CrazyTestWithMultiThreadMultiTask\"");
                _ = loggerA.LogSystem($"This is the \"SYSTEM\" a message number the \"CrazyTestWithMultiThreadMultiTask\"");
                _ = loggerA.LogSystem($"This is the \"EXCEPTION\" a message from the \"CrazyTestWithMultiThreadMultiTask\"");


                List<Task> taskList = new List<Task>()
                {


                        Task.Run(()=>
                      {


                              _= loggerA.LogInfo($"This is the \"INFO\" message number  from the \"CrazyTestWithMultiThreadMultiTask\"",EventId:1);
                              _= loggerA.LogAlert($"This is the \"ALERT\" message number  from the \"CrazyTestWithMultiThreadMultiTask\"","ANPR",2);
                              _= loggerA.LogSystem($"This is the \"SYSTEM\" message number  from the \"CrazyTestWithMultiThreadMultiTask\"");
                              _= loggerA.LogSystem($"This is the \"EXCEPTION\" message number from the \"CrazyTestWithMultiThreadMultiTask\"");


                      }),


                        Task.Run(() =>
                        {






                _ = loggerA.LogException(new InvalidCastException("ANPR engine cast is not valid",
                                           new CannotUnloadAppDomainException("Engine is not loaded",
                                           new AccessViolationException("Just kiddin U !",
                                           new InvalidTimeZoneException("Really  kiddin With U !")))),1365);

                _ = loggerA.LogException(new InvalidOperationException(), 1365);
                _ = loggerA.LogException(new DivideByZeroException(), 0);
                _ = loggerA.LogException(new FileNotFoundException(), -1);

                _ = loggerA.LogInfo($"This is the \"INFO\" a message from the \"CrazyTestWithMultiThreadMultiTask\"");
                _ = loggerA.LogAlert($"This is the \"ALERT\" a message from the \"CrazyTestWithMultiThreadMultiTask\"");
                _ = loggerA.LogSystem($"This is the \"SYSTEM\" a message number the \"CrazyTestWithMultiThreadMultiTask\"");
                _ = loggerA.LogSystem($"This is the \"EXCEPTION\" a message from the \"CrazyTestWithMultiThreadMultiTask\"");


                        }),



                        Task.Run(() =>
                        {


                               _= loggerA.LogError($"This is the \"ERROR\" message from the \"CrazyTestWithMultiThreadMultiTask\"");
                               _= loggerA.LogDebug($"This is the \"DEBUG\" message from the \"CrazyTestWithMultiThreadMultiTask\"");
                               _= loggerA.LogInfo($"This is the \"INFO\" message from the \"CrazyTestWithMultiThreadMultiTask\"");
                               _= loggerA.LogNote($"This is the \"NOTE\" message from the \"CrazyTestWithMultiThreadMultiTask\"");


                             _ = loggerA.LogInfo($"This is the \"INFO\" a message from the \"CrazyTestWithMultiThreadMultiTask\"");
                _ = loggerA.LogAlert($"This is the \"ALERT\" a message from the \"CrazyTestWithMultiThreadMultiTask\"");
                _ = loggerA.LogSystem($"This is the \"SYSTEM\" a message number the \"CrazyTestWithMultiThreadMultiTask\"");
                _ = loggerA.LogSystem($"This is the \"EXCEPTION\" a message from the \"CrazyTestWithMultiThreadMultiTask\"");



                        }),


                   Task.Run(() =>
                        {


                               _= loggerA.LogError($"This is the \"ERROR\" message from the \"CrazyTestWithMultiThreadMultiTask\"");
                               _= loggerA.LogDebug($"This is the \"DEBUG\" message from the \"CrazyTestWithMultiThreadMultiTask\"");
                               _= loggerA.LogInfo($"This is the \"INFO\" message from the \"CrazyTestWithMultiThreadMultiTask\"");
                               _= loggerA.LogNote($"This is the \"NOTE\" message from the \"CrazyTestWithMultiThreadMultiTask\"");

                               _= loggerA.LogException(new Exception($"This is a \"Test Exception\" from \"CrazyTestWithMultiThreadMultiTask\""));
                                _= loggerA.LogSecurity($"This is a \"Test Security\" number  from \"CrazyTestWithMultiThreadMultiTask\"");
                                _= loggerA.LogSystem($"This is a \"Test SYSTEM\" number  from \"CrazyTestWithMultiThreadMultiTask\"");
                                _= loggerA.LogSecurity($"This is a \"Test Security\"  from \"CrazyTestWithMultiThreadMultiTask\"");
                                _= loggerA.LogSystem($"This is a \"Test SYSTEM\"  from \"CrazyTestWithMultiThreadMultiTask\"");




                _ = loggerA.LogException(new InvalidCastException(), 1364);

                        }),

                      Task.Run(() =>
                        {


                               _= loggerA.LogError($"This is the \"ERROR\" message from the \"CrazyTestWithMultiThreadMultiTask\"");


                _ = loggerA.LogInfo($"This is the \"INFO\" a message from the \"CrazyTestWithMultiThreadMultiTask\"");
                _ = loggerA.LogAlert($"This is the \"ALERT\" a message from the \"CrazyTestWithMultiThreadMultiTask\"");
                _ = loggerA.LogSystem($"This is the \"SYSTEM\" a message number the \"CrazyTestWithMultiThreadMultiTask\"");
                _ = loggerA.LogSystem($"This is the \"EXCEPTION\" a message from the \"CrazyTestWithMultiThreadMultiTask\"");

                        })

                };


                // await Task.WhenAll(taskList);

            });




        }



        public static void NormalTest()
        {
            while (true)
            {

                Exception exception = new InvalidCastException("ANPR engine cast is not valid",
                                        new CannotUnloadAppDomainException("Engine is not loaded", new AccessViolationException("Just kiddin U !")));


                _ = loggerA.LogException(exception, 1364);

                Task.Delay(50).GetAwaiter().GetResult();

                _ = loggerA.LogInfo($"This is an \"INFO\" message from the \"CrazyTestWithMultiThreadMultiTask\"");
                Task.Delay(50).GetAwaiter().GetResult();

                _ = loggerA.LogAlert($"This is an \"ALERT\" message from the \"CrazyTestWithMultiThreadMultiTask\"");
                Task.Delay(50).GetAwaiter().GetResult();

                _ = loggerA.LogError($"This is an \"ERROR\" message from the \"CrazyTestWithMultiThreadMultiTask\"");

                Task.Delay(50).GetAwaiter().GetResult();

                _ = loggerA.LogDebug($"This is a \"DEBUG\" message from the \"CrazyTestWithMultiThreadMultiTask\"");

                Task.Delay(50).GetAwaiter().GetResult();


                _ = loggerA.LogWarning($"This is a \"WARNING\" message from the \"CrazyTestWithMultiThreadMultiTask\"");

                Task.Delay(50).GetAwaiter().GetResult();


                _ = loggerA.LogSystem($"This is a \"SYSTEM\" message from the \"CrazyTestWithMultiThreadMultiTask\"");

                Task.Delay(50).GetAwaiter().GetResult();


                _ = loggerA.LogSecurity($"This is a \"SECURITY\" message from the \"CrazyTestWithMultiThreadMultiTask\"");

                Task.Delay(50).GetAwaiter().GetResult();




            }

        }



    }

}
