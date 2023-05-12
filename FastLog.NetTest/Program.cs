using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using TrendSoft.FastLog.Agents;
using TrendSoft.FastLog.Core;

namespace TrendSoft.LogModuleTest
{
    internal static class Program
    {

        private static Logger Logger;
        private static Task loggerTask;

        static async Task Main(string[] args)
        {
            InitializeLogger();

            _ = LoggerWriteTest();

            await loggerTask;
        }


        private static void InitializeLogger()
        {
            Logger = new Logger("D:\\LoggerInternalException.txt",LogMachineName:false);

            Logger.AddLoggingAgent(new PlainTextFileLogger("D:\\PlainTextLogs.log"));
            //Logger.AddLoggingAgent(new ConsoleLogger());
            //Logger.AddLoggingAgent(new DebugWindowLogger());


            loggerTask = Logger.StartLogger();

        }


        private static async Task LoggerWriteTest()
        {
            List<Task> taskList = new List<Task>()
                    {


                        Task.Run(()=>
                      {

                          for (int i = 0; i < 5_000; i++)
                          {
                              _= Logger.LogInfo($"This is the \"INFO\" message number {i:N0} from the \"LoggerWriteTest\"");
                              _= Logger.LogError($"This is the \"ERROR\" message number {i:N0} from the \"LoggerWriteTest\"");
                              _= Logger.LogDebug($"This is the \"DEBUG\" message number {i:N0} from the \"LoggerWriteTest\"");
                              _= Logger.LogWarning($"This is the \"WARNING\" message number {i:N0} from the \"LoggerWriteTest\"");
                          }


                      }),


                        Task.Run(() =>
                        {

                            for (int i = 0; i < 5_000; i++)
                            {
                               _= Logger.LogException(new Exception($"This is a \"Test Exception\" number {i:N0} from \"LoggerWriteTest\""));
                            }
                        })



                    };


            await Task.WhenAll(taskList);

            Console.Beep();


            await loggerTask;


        }


    }



}

