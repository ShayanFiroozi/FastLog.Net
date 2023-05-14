using FastLog.Net.Enums;
using FastLog.NetTest;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using TrendSoft.FastLog.Agents;
using TrendSoft.FastLog.Core;
using TrendSoft.FastLog.InternalException;
using TrendSoft.FastLog.Models;

namespace TrendSoft.LogModuleTest
{
    internal static class Program
    {

        static async Task Main(string[] args)
        {

            LoggerTest.InitLoggers();

            LoggerTest.CrazyTestWithMultiThreadMultiTask();

            Console.Beep();

            Console.ReadLine();

        }



    }


}



