
using NUnit.Framework;
using System;
using System.Diagnostics;
using System.Threading.Tasks;
using TrendSoft.LogModule.InternalException;

namespace TrendSoft.LogModule.Test
{
    public class InternalExceptionTest
    {

        [SetUp]
        public void CreateInternalLogger()
        {
            InternalExceptionLogger.SetLogFile("D:\\InternalExceptions.log");
            InternalExceptionLogger.SetLogFileMaxSizeMB(100);
            InternalExceptionLogger.ReflectOnConsole();
            _ = InternalExceptionLogger.StartLogger();
            
        }

        [Test]
        public async Task InternalExceptionsLoggerTest()
        {
            for (int i = 0; i < 100_000; i++)
            {
                InternalExceptionLogger.LogInternalException(new Exception($"This is a \"Test Exception\" number {i:N0} from \"InternalExceptionsLoggerTest\""));
            }


            for (int i = 0; i < 100_000; i++)
            {
                InternalExceptionLogger.LogInternalException(new Exception($"This is a \"Test Exception\" number {i:N0} from \"InternalExceptionsLoggerTest\""));
            }

            Console.Beep();

            await Task.Delay(-1);

        }





    }
}