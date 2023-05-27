using FastLog.NetTest;
using System;
using System.Threading.Tasks;

namespace LogModuleTest
{
    internal static class Program
    {

        static async Task Main(string[] args)
        {

            LoggerTest.StartLoggers();

            //Task.Delay(1_000).GetAwaiter().GetResult();


            // LoggerTest.CrazyTestWithMultiThreadMultiTask();


            // Test With 5 thread ( to test thread-safety)
            for (int i = 0; i < 5; i++)
            {
                _= Task.Run(LoggerTest.NormalTest);
            }

            await Task.Run(LoggerTest.NormalTest);

            Console.Beep();

            Console.ReadLine();

        }



    }


}



