using FastLog.NetTest;
using System;
using System.Threading.Tasks;
using TrendSoft.FastLog.Agents;

namespace TrendSoft.LogModuleTest
{
    internal static class Program
    {

        static void Main(string[] args)
        {

            LoggerTest.StartLoggers();

            Task.Delay(5_000).GetAwaiter().GetResult();


            LoggerTest.CrazyTestWithMultiThreadMultiTask();

            Console.Beep();

            Console.ReadLine();

        }



    }


}



