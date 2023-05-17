using System;
using System.IO;
using System.Threading.Tasks;
using TrendSoft.FastLog.Internal;

namespace FastLog.NetTest
{
    internal static class InternalLoggerTest
    {


        public static readonly InternalLogger InternalLoggerAgent = InternalLogger
                                                          .Create()
                                                          .SaveInternalEventsToFile("D:\\Logs\\InternalLogger.LOG")
                                                          .DeleteTheLogFileWhenExceededTheMaximumSizeOf(100);


        public static void CrazyTestMultiThreadWithSameLogFile()
        {
            Parallel.For(0, 50_000, (i) =>
            {
                InternalLoggerAgent.LogInternalException(new InvalidCastException());
                InternalLoggerAgent.LogInternalException(new InvalidOperationException());
                InternalLoggerAgent.LogInternalException(new DivideByZeroException());
                InternalLoggerAgent.LogInternalException(new FileNotFoundException());
            });


            Task.Delay(20_000).GetAwaiter().GetResult();

        }

    }
}
