using FastLog.Net.Enums;
using FastLog.Net.Helpers;
using System;
using System.Diagnostics;
using System.IO;
using System.Runtime.InteropServices;
using TrendSoft.FastLog.Models;

namespace TrendSoft.FastLog.InternalException
{
    // Note : This class uses "fluent builder" pattern.


    public class InternalExceptionLogger
    {


        #region Properties
        private string InternalExceptionsLogFile { get; set; } = string.Empty;
        private short InternalExceptionsMaxLogFileSizeMB { get; set; } = 0;

        private bool _LogOnConsole { get; set; } = false;
        private bool _LogOnDebugWindow { get; set; } = false;

        private bool _Beep { get; set; } = false;

        #endregion


        #region Fluent Builder functions
        private InternalExceptionLogger()
        {
            //Keep it private to make it non accessible from the outside of the class !!
        }

        public static InternalExceptionLogger Create() => new InternalExceptionLogger();


        public InternalExceptionLogger SaveExceptionsLogToFile(string filename)
        {
            if (string.IsNullOrWhiteSpace(filename))
            {
                throw new ArgumentException($"'{nameof(filename)}' cannot be null or whitespace.", nameof(filename));
            }


            InternalExceptionsLogFile = filename;


            if (!Directory.Exists(Path.GetDirectoryName(InternalExceptionsLogFile)))
            {
                _ = Directory.CreateDirectory(Path.GetDirectoryName(InternalExceptionsLogFile));
            }


            return this;


        }

        public InternalExceptionLogger NotBiggerThan(short logFileMaxSize)
        {

            if (logFileMaxSize <= 0)
            {
                throw new ArgumentException($"'{nameof(logFileMaxSize)}' must be greater then zero.", nameof(logFileMaxSize));
            }

            InternalExceptionsMaxLogFileSizeMB = logFileMaxSize;

            return this;

        }


        public InternalExceptionLogger PrintOnConsole()
        {
            _LogOnConsole = true;
            return this;
        }
        public InternalExceptionLogger DoNotPrintOnConsole()
        {
            _LogOnConsole = false;
            return this;
        }

        public InternalExceptionLogger Beep()
        {
            _Beep = true;
            return this;
        }
        public InternalExceptionLogger DoNotBeep()
        {
            _Beep = false;
            return this;
        }

        public InternalExceptionLogger PrintOnDebugWindow()
        {
            _LogOnDebugWindow = true;
            return this;
        }
        public InternalExceptionLogger DoNotPrintOnDebugWindow()
        {
            _LogOnDebugWindow = false;
            return this;
        }
        #endregion





        public void LogInternalException(Exception exception)
        {
            if (string.IsNullOrWhiteSpace(InternalExceptionsLogFile))
            {
                throw new ArgumentException($"'{nameof(InternalExceptionsLogFile)}' cannot be null or whitespace.", nameof(InternalExceptionsLogFile));
            }

            if (InternalExceptionsMaxLogFileSizeMB <= 0)
            {
                throw new ArgumentException($"'{nameof(InternalExceptionsMaxLogFileSizeMB)}' must be greater then zero.", nameof(InternalExceptionsMaxLogFileSizeMB));
            }

            if (exception is null)
            {
                return;
            }

            try
            {

                try
                {
                    CheckInternalExceptionsLogFileSize();
                }
                catch { }


                LogEventModel LogToSave = new LogEventModel(LogEventTypes.EXCEPTION,

                   $"\nId : {exception.HResult}\n" +
                   $"Message : {exception.Message ?? "N/A"}\n",

                   $"InnerException : {exception.InnerException?.Message ?? "N/A"}\n" +
                   $"StackTrace : {exception.StackTrace ?? "N/A"}",

                   $"Source : {exception.Source ?? "N/A"}\n" +
                   $"Target Site : {(exception.TargetSite != null ? exception.TargetSite.Name : "N/A")}");



                if (_LogOnConsole)
                {
                    Console.WriteLine();
                    Console.BackgroundColor = ConsoleColor.Red;
                    Console.Write($"Logger \"Internal Exception\" has been occured :");
                    Console.ResetColor();
                    Console.ForegroundColor = ConsoleColor.DarkRed;

                    Console.WriteLine($"{LogToSave.GetLogMessage(true)}\n");
                }


                if (_LogOnDebugWindow)
                {
                    Debug.WriteLine($"\nLogger \"Internal Exception\" has been occured :");
                    Debug.WriteLine($"{LogToSave.GetLogMessage(true)}\n");
                }



                // ATTENTION : there's a chance of "HostProtectionException" exception.

                // For more info please visit : https://learn.microsoft.com/en-us/dotnet/api/system.console.beep?view=net-7.0

                try
                {
                    if (_Beep)
                    {
                        // Note : "Beep" works only on Windows® OS.
                        if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows)) Console.Beep();
                    }
                }
                catch { }




                ThreadSafeFileHelper.AppendAllText(InternalExceptionsLogFile,
                                                $"{LogToSave.GetLogMessage(true)}\n");


                // May be NOT "Thread-Safe"
                //File.AppendAllText(InternalExceptionsLogFile, $"{LogToSave.GetLogMessage(true)}\n");

            }
            catch
            {
            }



        }





        #region Private Functions
        private short GetLogFileSizeMB()
        {
            try
            {
                return string.IsNullOrWhiteSpace(InternalExceptionsLogFile) ? (short)0 : !File.Exists(InternalExceptionsLogFile) ? (short)0 :
                (short)(new FileInfo(InternalExceptionsLogFile).Length / 1024 / 1024);
            }
            catch
            {
                return 0;
            }
        }

        private void CheckInternalExceptionsLogFileSize()
        {
            try
            {
                if (string.IsNullOrWhiteSpace(InternalExceptionsLogFile))
                {
                    return;
                }

                if (!File.Exists(InternalExceptionsLogFile))
                {
                    return;
                }

                if (GetLogFileSizeMB() >= InternalExceptionsMaxLogFileSizeMB)
                {
                    ThreadSafeFileHelper.DeleteFile(InternalExceptionsLogFile);

                    // May be NOT "Thread-Safe"
                    //File.Delete(InternalExceptionsLogFile);
                }
            }
            catch
            {

            }

        }
        #endregion





    }

}

