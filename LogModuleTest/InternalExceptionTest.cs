
using NUnit.Framework;
using System;
using System.Threading.Tasks;
using TrendSoft.LogModule.InternalException;

namespace TrendSoft.LogModule.Test
{
    public class InternalExceptionTest
    {

        [SetUp]
        public void CreateInternalLogger()
        {
            InternalExceptionLogger.SetLogFile("InternalExceptions.log");
            InternalExceptionLogger.SetLogFileMaxSizeMB(100);
            InternalExceptionLogger.ReflectOnConsole();
            _ = InternalExceptionLogger.StartLogger();

        }

        [Test]
        public async Task InternalExceptionsLoggerTest()
        {
            for (int i = 0; i < 10_000; i++)
            {
                _ = InternalExceptionLogger.LogInternalException(new Exception($"This is a \"Test Exception\" number {i:N0} from \"InternalExceptionsLoggerTest\""));
            }


            for (int i = 0; i < 10_000; i++)
            {
                _ = InternalExceptionLogger.LogInternalException(new Exception($"This is a \"Test Exception\" number {i:N0} from \"InternalExceptionsLoggerTest\""));
            }

            Console.Beep();

            await Task.Delay(-1);

        }





    }
}