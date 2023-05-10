
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
            InternalExceptionLogger.SetLogFile("D:\\InternalExceptions.log");
            InternalExceptionLogger.SetLogFileMaxSizeMB(100);
            InternalExceptionLogger.DoReflectOnConsole();
            

        }

        [Test]
        public void InternalExceptionsLoggerTest()
        {

            for (int i = 0; i < 10_000; i++)
            {
                InternalExceptionLogger.LogInternalException(new Exception($"This is a \"Test Exception\" number {i:N0} from \"InternalExceptionsLoggerTest\""));
            }


            for (int i = 0; i < 10_000; i++)
            {
                InternalExceptionLogger.LogInternalException(new Exception($"This is a \"Test Exception\" number {i:N0} from \"InternalExceptionsLoggerTest\""));
            }

            Console.Beep();

          
        }





    }
}