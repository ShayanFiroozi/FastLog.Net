﻿using FastLog.Net.Enums;
using FastLog.Net.Helpers;
using System;
using System.Diagnostics;
using System.IO;
using System.Runtime.InteropServices;
using TrendSoft.FastLog.Models;

namespace TrendSoft.FastLog.Internal
{
    // Note : This class uses "fluent builder" pattern.


    public class InternalLogger
    {


        #region Properties
        private string InternalLogFile { get; set; } = string.Empty;
        private short InternalExceptionsMaxLogFileSizeMB { get; set; } = 0;

        private bool _PrintOnConsole { get; set; } = false;
        private bool _LogOnDebugWindow { get; set; } = false;

        private bool _Beep { get; set; } = false;

        private bool _BeepOnlyOnDebugMode { get; set; } = false;

        private bool _PrintOnConsoleOnlyOnDebugMode { get; set; } = false;



        #endregion


        #region Fluent Builder functions
        private InternalLogger()
        {
            //Keep it private to make it non accessible from the outside of the class !!
        }

        public static InternalLogger Create() => new InternalLogger();


        public InternalLogger SaveInternalEventsToFile(string filename)
        {
            if (!string.IsNullOrWhiteSpace(InternalLogFile))
            {
                throw new ArgumentException($"'{nameof(filename)}' cannot add two or more file.", nameof(filename));
            }

            if (string.IsNullOrWhiteSpace(filename))
            {
                throw new ArgumentException($"'{nameof(filename)}' cannot be null or whitespace.", nameof(filename));
            }


            InternalLogFile = filename;


            if (!Directory.Exists(Path.GetDirectoryName(InternalLogFile)))
            {
                _ = Directory.CreateDirectory(Path.GetDirectoryName(InternalLogFile));
            }


            return this;


        }


        public InternalLogger DeleteTheLogFileWhenExceededTheMaximumSizeOf(short logFileMaxSizeMB)
        {

            if (logFileMaxSizeMB <= 0)
            {
                throw new ArgumentException($"'{nameof(logFileMaxSizeMB)}' must be greater then zero.", nameof(logFileMaxSizeMB));
            }

            InternalExceptionsMaxLogFileSizeMB = logFileMaxSizeMB;

            return this;

        }


        public InternalLogger PrintOnConsole()
        {
            _PrintOnConsole = true;
            return this;
        }


        public InternalLogger PrintOnConsoleOnlyOnDebugMode()
        {
            if (!_PrintOnConsole)
            {
                throw new ArgumentException($"Console Print is not active. call \"PrintOnConsole\" first.");
            }

            _PrintOnConsoleOnlyOnDebugMode = true;
            return this;
        }

        public InternalLogger DoNotPrintOnConsole()
        {
            _PrintOnConsole = false;
            return this;
        }

        public InternalLogger Beep()
        {

            _Beep = true;
            return this;
        }

        public InternalLogger BeepOnlyOnDebugMode()
        {
            if (!_Beep)
            {
                throw new ArgumentException($"Beep is not active. call \"Beep\" first.");
            }

            _BeepOnlyOnDebugMode = true;
            return this;
        }
        public InternalLogger DoNotBeep()
        {
            _Beep = false;
            return this;
        }

        public InternalLogger PrintOnDebugWindow()
        {
            _LogOnDebugWindow = true;
            return this;
        }
        public InternalLogger DoNotPrintOnDebugWindow()
        {
            _LogOnDebugWindow = false;
            return this;
        }
        #endregion





        public void LogInternalException(Exception exception)
        {
            if (string.IsNullOrWhiteSpace(InternalLogFile))
            {
                throw new ArgumentException($"'{nameof(InternalLogFile)}' cannot be null or whitespace.", nameof(InternalLogFile));
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



                if (_PrintOnConsole)
                {
                    if (_PrintOnConsoleOnlyOnDebugMode)
                    {
#if DEBUG
                        Console.WriteLine();
                        Console.BackgroundColor = ConsoleColor.Red;
                        Console.Write($"Logger \"Internal Exception\" has been occured :");
                        Console.ResetColor();
                        Console.ForegroundColor = ConsoleColor.DarkRed;

                        Console.WriteLine($"{LogToSave.GetLogMessage(true)}\n");

                        Console.ResetColor();
#endif
                    }
                    else
                    {
                        Console.WriteLine();
                        Console.BackgroundColor = ConsoleColor.Red;
                        Console.Write($"Logger \"Internal Exception\" has been occured :");
                        Console.ResetColor();
                        Console.ForegroundColor = ConsoleColor.DarkRed;

                        Console.WriteLine($"{LogToSave.GetLogMessage(true)}\n");

                        Console.ResetColor();
                    }
                }


                if (_LogOnDebugWindow)
                {
                    Debug.WriteLine($"\nLogger \"Internal Exception\" has been occured :");
                    Debug.WriteLine($"{LogToSave.GetLogMessage(true)}\n");
                }



                // ATTENTION : There's a chance of "HostProtectionException" or "PlatformNotSupportedException" exception.

                // For more info please visit : https://learn.microsoft.com/en-us/dotnet/api/system.console.beep?view=net-7.0

                try
                {
                    if (_Beep)
                    {
                        if (_BeepOnlyOnDebugMode)
                        {
#if DEBUG
                            // Note : "Beep" works only on Windows® OS.
                            if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows)) Console.Beep();
#endif
                        }
                        else
                        {
                            // Note : "Beep" works only on Windows® OS.
                            if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows)) Console.Beep();
                        }
                    }
                }
                catch { }


                ThreadSafeFileHelper.AppendAllText(InternalLogFile,
                                                $"{LogToSave.GetLogMessage(true)}");


                // May be NOT "Thread-Safe"
                //File.AppendAllText(InternalExceptionsLogFile, $"{LogToSave.GetLogMessage(true)}\n");

            }
            catch
            {
            }



        }


        public void LogInternalSystemEvent(LogEventModel logEventModel)
        {

            try
            {

                if (_PrintOnConsole)
                {
                    Console.WriteLine();
                    Console.ForegroundColor = ConsoleColor.Yellow;
                    Console.Write($"Logger \"Internal System Event\" has been occured :\n");
                    Console.ResetColor();
                    Console.ForegroundColor = ConsoleColor.DarkGray;

                    Console.WriteLine($"{logEventModel.GetLogMessage(true)}");

                    Console.ResetColor();
                }


                if (_LogOnDebugWindow)
                {
                    Debug.Write($"Logger \"Internal System Event\" has been occured :\n");
                    Debug.WriteLine($"{logEventModel.GetLogMessage(true)}");
                }

                // ATTENTION : There's a chance of "HostProtectionException" or "PlatformNotSupportedException" exception.

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


                ThreadSafeFileHelper.AppendAllText(InternalLogFile,
                                                $"{logEventModel.GetLogMessage(true)}");


                // May be NOT "Thread-Safe"
                //File.AppendAllText(InternalExceptionsLogFile, $"{LogToSave.GetLogMessage(true)}\n");

            }
            catch
            {
            }



        }



        private void CheckInternalExceptionsLogFileSize()
        {
            try
            {
                if (string.IsNullOrWhiteSpace(InternalLogFile))
                {
                    return;
                }

                if (!File.Exists(InternalLogFile))
                {
                    return;
                }

                if (ThreadSafeFileHelper.GetFileSize(InternalLogFile) >= InternalExceptionsMaxLogFileSizeMB)
                {
                    ThreadSafeFileHelper.DeleteFile(InternalLogFile);

                    // May be NOT "Thread-Safe"
                    //File.Delete(InternalExceptionsLogFile);
                }
            }
            catch
            {

            }

        }



    }

}
