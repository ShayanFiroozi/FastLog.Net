using FastLog.NetTest;
using System;

namespace TrendSoft.LogModuleTest
{
    internal static class Program
    {

        static void Main(string[] args)
        {

            LoggerTest.InitLoggers();

            LoggerTest.CrazyTestWithMultiThreadMultiTask();

            Console.Beep();

            Console.ReadLine();

        }



    }


}



