/*---------------------------------------------------------------------------------------------

                ► FastLog.Net , High Performance Logger For .Net ◄



 → Copyright (c) 2020-2023 Shayan Firoozi , Bandar Abbas , Iran , Under MIT License.

 → Contact : <shayan.firoozi@gmail.com>

 → GitHub repository : https://github.com/ShayanFiroozi/FastLog.Net

---------------------------------------------------------------------------------------------*/

using FastLog.NetTest;
using System;
using System.Threading.Tasks;

namespace LogModuleTest
{
    internal static class Program
    {

        private static void printMenu(string[] options)
        {
            Console.Clear();
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine("FastLog.Net Test App -->");
            Console.ResetColor();
            Console.WriteLine("");

            Console.WriteLine("Please choose an action : \n");


            foreach (string option in options)
            {
                Console.WriteLine(option);
            }

            Console.Write("\nChoose your option : ");

        }

        public static async Task Main(string[] args)
        {
         
            string[] options = {
                                $"1- FastLog.Net \"Internal Logger\" Test With Multi Threats (10,000 tasks simultaneously).",
                                $"2- FastLog.Net \"Logger\" Test With Multi Threats (10,000 tasks simultaneously).",
                                "3- Exit",
                               };
            while (true)
            {
                printMenu(options);


                int option;
                try
                {
                    option = Convert.ToInt32(Console.ReadLine());
                }
                catch (FormatException)
                {
                    Console.WriteLine("Please enter an integer value between 1 and " + options.Length);
                    continue;
                }
                catch (Exception)
                {
                    Console.WriteLine("An unexpected error happened. Please try again");
                    continue;
                }
                switch (option)
                {
                    case 1:
                        Console.Clear();
                        await InternalLoggerTest.CrazyTestWithMultiTasks();
                        Console.WriteLine("\nPress any key to continue...");
                        Console.ReadLine();
                        break;
                    case 2:
                        Console.Clear();
                        await LoggerTest.CrazyTestWithMultiTasks();
                        Console.WriteLine("\nPress any key to continue...");
                        Console.ReadLine();
                        break;
                    case 3:
                        Environment.Exit(0);
                        break;
                    default:
                        Console.WriteLine("Please enter an integer value between 1 and " + options.Length);
                        break;
                }

            }
        }

    }


}



