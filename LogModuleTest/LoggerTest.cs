
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using TrendSoft.LogModule.Agents;
using TrendSoft.LogModule.Core;
using TrendSoft.LogModule.InternalException;
using TrendSoft.LogModule.Models;

namespace TrendSoft.LogModule.Test
{
    public class LoggerTest
    {

        private Logger Logger;
        private Task loggerTask;

        [SetUp]
        public void InitializeLogger()
        {
            Logger = new Logger("D:\\LoggerInternalException.txt");
            Logger.AddLoggingAgent(new PlainTextFileLogger("D:\\PlainTextLogs.log"));
            Logger.AddLoggingAgent(new ConsoleLogger());

            loggerTask = Logger.StartLogger();

        }

        [Test]
        public async Task LoggerWriteTest()
        {
            List<Task> taskList = new();

            taskList.Add(Task.Run(() =>
              {

                  for (int i = 0; i < 10_000; i++)
                  {
                      _ = Logger.LogInfo($"This is the INFO message number {i:N0} from the \"LoggerWriteTest\"");
                      _ = Logger.LogError($"This is the ERROR message number {i:N0} from the \"LoggerWriteTest\"");
                      _ = Logger.LogDebug($"This is the DEBUG message number {i:N0} from the \"LoggerWriteTest\"");
                      _ = Logger.LogWarning($"This is the WARNING message number {i:N0} from the \"LoggerWriteTest\"");
                  }


              }));


            taskList.Add(Task.Run(() =>
            {

                for (int i = 0; i < 50_000; i++)
                {
                    _ = Logger.LogException(new Exception($"This is a \"Test Exception\" number {i:N0} from \"LoggerWriteTest\""));
                }
            }));


            await Task.WhenAll(taskList);


            Console.Beep();

            await loggerTask;


        }





    }
}