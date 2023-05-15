﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TrendSoft.FastLog.InternalException;

namespace FastLog.NetTest
{
    internal static class InternalExceptionLoggerTest
    {
        private static InternalExceptionLogger InternalExceptionLoggerA = InternalExceptionLogger
                                                           .Create()
                                                           .SaveExceptionsLogToFile("D:\\Logs\\InternalExceptionsTest.LOG")
                                                           .NotBiggerThan(100);



        private static InternalExceptionLogger InternalExceptionLoggerB = InternalExceptionLogger
                                                          .Create()
                                                          .SaveExceptionsLogToFile("D:\\Logs\\InternalExceptionsTest.LOG")
                                                          .NotBiggerThan(100);


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