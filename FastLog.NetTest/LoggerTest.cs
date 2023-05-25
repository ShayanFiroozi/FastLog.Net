using FastLog.Core;
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
                                                          .SaveInternalEventsToFile("M:\\Logs\\InternalEventsLog.log")
                                                          .DeleteTheLogFileWhenExceededTheMaximumSizeOf(100);
            //.Beep()
            //  .BeepOnlyOnDebugMode()
            //   .PrintOnConsole();
            // .PrintOnConsoleOnlyOnDebugMode()
            // .PrintOnDebugWindow();




            ConfigManager loggerConfig = ConfigManager.Create()
                                                      .WithLoggerName("ANPR® Logger")
                                                      .WithMaxEventsToKeepInMemory(1_000);
                                                      //.RunAgentsInParallelMode();



            loggerA = Logger.Create()
                            .WithInternalLogger(internalLogger)
                            .WithConfiguration(loggerConfig)
                            .WithAgents()

                                .AddTextFileAgent()
                                   .UseJsonFormat()
                                   .SaveLogToFile("M:\\Logs\\TestLog.json")
                                   .DeleteTheLogFileWhenExceededTheMaximumSizeOf(10)
                                    .BuildAgent()


                                .AddTextFileAgent()
                                   .UseJsonFormat()
                                   .SaveLogToFile("M:\\Logs\\TestLog2.json")
                                   .DeleteTheLogFileWhenExceededTheMaximumSizeOf(10)
                                    .BuildAgent()





            // .AddConsoleAgent().UseJsonFormat().BuildAgent()



            .BuildLogger();


            loggerA.OnEventOccured += LoggerA_OnEventOccured;
            loggerA.OnEventProcessed += LoggerA_OnEventProcessed;

            loggerA.StartLogger();



            //await Task.Run(() =>
            //    {
            //        while (true)
            //        {



            //            foreach (LogEventModel logEvent in loggerA.InMemoryEvents)
            //            {

            //                //Console.WriteLine(loggerA.InMemoryEvents.Count());
            //                //Console.WriteLine(loggerA.InMemoryEvents.First().EventMessage);


            //                //Console.WriteLine();

            //                // Just to access the property from another thread to test thread-satefy.
            //                int x = loggerA.InMemoryEvents.Count();
            //                string temp = loggerA.InMemoryEvents.First().EventMessage;

            //            }

            //            Console.WriteLine();
            //            Console.WriteLine($"Remaining event(s) to process : {loggerA.ChannelEventCount:N0}");
            //            Console.WriteLine($"Total event(s) added to Channel : {loggerA.ChannelTotalEventCount:N0}");
            //            Console.WriteLine($"Total processed event(s) : {loggerA.ChannelProcessedEventCount:N0}");

            //            Console.WriteLine();

            //            Task.Delay(1).GetAwaiter().GetResult();



            //        }

            //    });



        }

        private static void LoggerA_OnEventProcessed(object sender, LogEventModel e)
        {
            Console.WriteLine($"Event Processed : {e}");
        }

        private static void LoggerA_OnEventOccured(object sender, LogEventModel e)
        {
            Console.WriteLine($"Event occured : {e}");
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
            while (loggerA.ChannelTotalEventCount < 100000)
            {
                // TimeSpan waitTime = TimeSpan.FromMilliseconds(1);

                Exception exception = new InvalidCastException("ANPR engine cast is not valid",
                                        new CannotUnloadAppDomainException("Engine is not loaded", new AccessViolationException("Just kiddin U !")));


                _ = loggerA.LogException(exception, 1364);

                //Task.Delay(waitTime).GetAwaiter().GetResult();

                _ = loggerA.LogInfo($"This is an \"INFO\" message from the \"CrazyTestWithMultiThreadMultiTask\"");
                // Task.Delay(waitTime).GetAwaiter().GetResult();

                _ = loggerA.LogAlert($"This is an \"ALERT\" message from the \"CrazyTestWithMultiThreadMultiTask\"");
                //  Task.Delay(waitTime).GetAwaiter().GetResult();

                _ = loggerA.LogError($"This is an \"ERROR\" message from the \"CrazyTestWithMultiThreadMultiTask\"");

                //  Task.Delay(waitTime).GetAwaiter().GetResult();

                _ = loggerA.LogDebug($"This is a \"DEBUG\" message from the \"CrazyTestWithMultiThreadMultiTask\"");

                // Task.Delay(waitTime).GetAwaiter().GetResult();


                _ = loggerA.LogWarning($"This is a \"WARNING\" message from the \"CrazyTestWithMultiThreadMultiTask\"");

                // Task.Delay(waitTime).GetAwaiter().GetResult();


                _ = loggerA.LogSystem($"This is a \"SYSTEM\" message from the \"CrazyTestWithMultiThreadMultiTask\"");

                // Task.Delay(waitTime).GetAwaiter().GetResult();


                _ = loggerA.LogSecurity($"This is a \"SECURITY\" message from the \"CrazyTestWithMultiThreadMultiTask\"");


                _ = loggerA.LogException(new Exception("Test Exception !"));

                // Task.Delay(waitTime).GetAwaiter().GetResult();




            }

        }



    }

}
