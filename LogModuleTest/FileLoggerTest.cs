
using NUnit.Framework;
using System;
using System.Threading.Tasks;
using TrendSoft.LogModule.Agents;
using TrendSoft.LogModule.InternalException;
using TrendSoft.LogModule.Models;

namespace TrendSoft.LogModule.Test
{
    public class FileLoggerTest
    {

        private PlainTextFileLogger FileLogger;

        [SetUp]
        public void InitializeLoggers()
        {
            InternalExceptionLogger.SetLogFile("D:\\InternalExceptions.log");
            InternalExceptionLogger.SetLogFileMaxSizeMB(100);


            FileLogger = new PlainTextFileLogger("D:\\FileLogger.log");




        }

        [Test]
        public async Task FileLoggerWriteTest()
        {

            for (int i = 0; i < 10_000; i++)
            {
              await FileLogger.LogEvent(new LogEventModel(LogEventModel.LogTypeEnum.INFO, $"This is a Test Info Log number {i}"));
            }


            for (int i = 0; i < 20_000; i++)
            {
                await FileLogger.LogEvent(new LogEventModel(new Exception($"This is a \"Test Exception\" number {i:N0} from \"FileLoggerWriteTest\"")));
            }

            Console.Beep();


        }





    }
}