using System;
using System.IO;
using System.Threading.Tasks;
using TrendSoft.FastLog.Internal;

namespace FastLog.NetTest
{
    internal static class InternalExceptionLoggerTest
    {
        private static readonly InternalLogger InternalExceptionLoggerA = InternalLogger
                                                           .Create()
                                                           .SaveInternalEventsToFile("D:\\Logs\\InternalExceptionsTest.LOG")
                                                           .DeleteTheLogFileIfExceededTheMaximumSizeOf(100);



        private static readonly InternalLogger InternalExceptionLoggerB = InternalLogger
                                                          .Create()
                                                          .SaveInternalEventsToFile("D:\\Logs\\InternalExceptionsTest.LOG")
                                                          .DeleteTheLogFileIfExceededTheMaximumSizeOf(100);


        public static void CrazyTestWithSameLogFile()
        {
            Parallel.For(0, 50_000, (i) =>
            {
                InternalExceptionLoggerA.LogInternalException(new InvalidCastException());
                InternalExceptionLoggerB.LogInternalException(new InvalidOperationException());
                InternalExceptionLoggerA.LogInternalException(new DivideByZeroException());
                InternalExceptionLoggerB.LogInternalException(new FileNotFoundException());
            });


            Task.Delay(20_000).GetAwaiter().GetResult();

        }

    }
}
