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

            Task.Delay(500).GetAwaiter().GetResult();

            LoggerTest.CrazyTestWithMultiThreadMultiTask();

            Console.Beep();

            Console.ReadLine();

        }



    }


}



