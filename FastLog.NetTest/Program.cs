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
                                "1- Internal Logger Test With Multi Threats (10,000 tasks simultaneously).",
                                "2- Logger Test With Multi Threats (1,000 tasks simultaneously).",
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
                        await InternalLoggerTest.CrazyTestMultiTasks();
                        Console.WriteLine("\nPress any key to continue...");
                        Console.ReadLine();
                        break;
                    case 2:
                        //option2();
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



        //static async Task Main(string[] args)
        //{

        //    Console.WriteLine("");

        //    Console.ForegroundColor = ConsoleColor.Yellow;
        //    Console.WriteLine("FastLog.Net Test Module");

        //    Console.ResetColor();

        //    Console.WriteLine("");

        //    Console.WriteLine("Choose");

        //    await InternalLoggerTest.CrazyTestMultiThreadWithSameLogFile();

        //    Console.Beep();

        //    Console.ReadLine();




        //    //LoggerTest.StartLoggers();




        //    //// Test With 5 thread ( to test thread-safety)
        //    //for (int i = 0; i < 5; i++)
        //    //{
        //    //    _ = Task.Run(LoggerTest.NormalTest);
        //    //}

        //    //await Task.Run(LoggerTest.NormalTest);



        //}



    }


}



