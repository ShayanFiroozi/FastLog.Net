using FastLog.NetTest;
using System;
using System.Threading.Tasks;

namespace TrendSoft.LogModuleTest
{
    internal static class Program
    {

        static void Main(string[] args)
        {

            LoggerTest.StartLoggers();

            Task.Delay(1_000).GetAwaiter().GetResult();


            LoggerTest.CrazyTestWithMultiThreadMultiTask();
          //  LoggerTest.NormalTest();

            Console.Beep();

            Console.ReadLine();

        }



    }


}



