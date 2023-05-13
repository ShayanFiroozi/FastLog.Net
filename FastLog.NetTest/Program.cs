using FastLog.Net.Enums;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using TrendSoft.FastLog.Agents;
using TrendSoft.FastLog.Core;
using TrendSoft.FastLog.Models;

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
            Logger = new Logger("D:\\Logs\\LoggerInternalException.txt", LogMachineName: false);

            Logger.AddLoggingAgent(new PlainTextFileLogger("D:\\Logs\\PlainTextLogsA.log"));
            Logger.AddLoggingAgent(new PlainTextFileLogger("D:\\Logs\\PlainTextLogsB.log"));
            Logger.AddLoggingAgent(new PlainTextFileLogger("D:\\Logs\\PlainTextLogsC.log"));
            Logger.AddLoggingAgent(new PlainTextFileLogger("D:\\Logs\\PlainTextLogsD.log"));


            // Add agent(s) to the Logger


            Logger.AddLoggingAgent(ConsoleLogger.Create());

#if DEBUG
            Logger.AddLoggingAgent(new HeavyOperationSimulator(TimeSpan.FromSeconds(5)));
#endif

            Logger.AddLoggingAgent(new WindowsEventLogger("Shayan"));

            loggerTask = Logger.StartLogger();

        }


        private static async Task LoggerWriteTest()
        {
            List<Task> taskList = new List<Task>()
                    {


                        Task.Run(()=>
                      {

                          for (int i = 0; i < 1_000; i++)
                          {
                              _= Logger.LogInfo($"This is the \"INFO\" message number {i:N0} from the \"LoggerWriteTest\"");
                              _= Logger.LogError($"This is the \"ERROR\" message number {i:N0} from the \"LoggerWriteTest\"");
                              _= Logger.LogDebug($"This is the \"DEBUG\" message number {i:N0} from the \"LoggerWriteTest\"");
                              _= Logger.LogWarning($"This is the \"WARNING\" message number {i:N0} from the \"LoggerWriteTest\"");
                              _= Logger.LogAlert($"This is the \"ALERT\" message number {i:N0} from the \"LoggerWriteTest\"");
                              _= Logger.LogSystem($"This is the \"SYSTEM\" message number {i:N0} from the \"LoggerWriteTest\"");
                              _= Logger.LogSystem($"This is the \"EXCEPTION\" message number {i:N0} from the \"LoggerWriteTest\"");
                          }


                      }),


                        Task.Run(() =>
                        {

                            for (int i = 0; i < 1_000; i++)
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

