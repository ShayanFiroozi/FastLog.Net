/*---------------------------------------------------------------------------------------------

                ► FastLog.Net , High Performance Logger For .Net ◄



 → Copyright (c) 2020-2023 Shayan Firoozi , Bandar Abbas , Iran , Under MIT License.

 → Contact : <shayan.firoozi@gmail.com>

 → GitHub repository : https://github.com/ShayanFiroozi/FastLog.Net

---------------------------------------------------------------------------------------------*/

using FastLog.Helpers;
using FastLog.Helpers.ExtendedMethods;
using FastLog.Interfaces;
using FastLog.Models;
using FluentConsoleNet;
using System;
using System.Diagnostics;
using System.IO;
using System.Runtime.InteropServices;
using System.Threading;

namespace FastLog.Internal
{



    /// <summary>
    /// Internal Logger class , resposible to log the exceptions and events which occured in the FastLog.Net
    /// Note : This class used Fluent Builder.
    /// </summary>
    public sealed class InternalLogger
    {


        #region Properties
        private string InternalLogFile { get; set; } = string.Empty;
        private short InternalExceptionsMaxLogFileSizeMB { get; set; } = 0;

        private bool _PrintOnConsole { get; set; } = false;
        private bool _LogOnDebugWindow { get; set; } = false;

        private bool _Beep { get; set; } = false;

        private bool _BeepOnlyOnDebugMode { get; set; } = false;

        private bool _PrintOnConsoleOnlyOnDebugMode { get; set; } = false;

        private bool useJsonFormat { get; set; } = false;


        private long totalLogCount = 0;

        public long TotalLogCount => totalLogCount;


        #endregion


        #region Fluent Builder functions
        private InternalLogger()
        {
            //Keep it private to make it non accessible from the outside of the class !!
        }

        public static InternalLogger Create() => new InternalLogger();

        public InternalLogger UseJsonFormat()
        {
            useJsonFormat = true;
            return this;
        }

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




        /// <summary>
        /// Log internal Exceptions of FastLog.Net
        /// </summary>
        /// <param name="exception"></param>
        /// <exception cref="ArgumentException"></exception>
        public void LogInternalException(Exception exception)
        {

            if (exception is null)
            {
                return;
            }

            try
            {

                Interlocked.Increment(ref totalLogCount);

                if (!string.IsNullOrWhiteSpace(InternalLogFile))
                {
                    CheckLogFileSize();
                }



                ILogEventModel LogToSave = new LogEventModel(exception);



                PrintOnConsole(LogToSave);


                if (_LogOnDebugWindow)
                {
                    Debug.WriteLine($"\nLogger \"Internal Exception\" has been occured :");
                    Debug.WriteLine($"{LogToSave.ToJsonText()}\n");
                }



                if (!string.IsNullOrWhiteSpace(InternalLogFile))
                {
                    ThreadSafeFileHelper.AppendAllText(InternalLogFile, useJsonFormat ? LogToSave.ToJsonText() : LogToSave.ToPlainText());
                }


                MakeBeepSound();


            }
            catch
            {
                // Can not catch Internal Logger's exceptions bevause the Internal Logger itself is design to catch and log the FastLoger.Net exceptions and events !!
            }



        }


        /// <summary>
        /// Log internal events of FastLog.Net
        /// </summary>
        /// <param name="logEventModel"></param>
        public void LogInternalSystemEvent(ILogEventModel logEventModel)
        {

            try
            {
                Interlocked.Increment(ref totalLogCount);

                if (!string.IsNullOrWhiteSpace(InternalLogFile))
                {
                    CheckLogFileSize();
                }


                if (_PrintOnConsole)
                {
                    FluentConsole.Console
                                 .AddLineBreak(1)
                                 .Write("Logger").AddSpace()
                                 .WithFontColor(ConsoleColor.Yellow)
                                 .Write("Internal System Event")
                                 .ResetColor()
                                 .AddSpace()
                                 .WriteLine("has been occured :")
                                 .WriteLine(useJsonFormat ? logEventModel.ToJsonText() : logEventModel.ToPlainText())
                                 .Print();

                }


                if (_LogOnDebugWindow)
                {
                    Debug.Write($"Logger \"Internal System Event\" has been occured :\n");
                    Debug.WriteLine(useJsonFormat ? logEventModel.ToJsonText() : logEventModel.ToPlainText());
                }

                // Note : "Beep" only works on Windows® OS.
                // ATTENTION : There's a possibility of "HostProtectionException" or "PlatformNotSupportedException" exception.
                // For more info please visit : https://learn.microsoft.com/en-us/dotnet/api/system.console.beep?view=net-7.0

                try
                {
                    if (_Beep && RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
                        Console.Beep();
                }
                catch
                {
                    // Ignore the Beep exceptions.
                }

                if (!string.IsNullOrWhiteSpace(InternalLogFile))
                {
                    ThreadSafeFileHelper.AppendAllText(InternalLogFile,
                                                useJsonFormat ? logEventModel.ToJsonText() : logEventModel.ToPlainText());
                }

            }
            catch
            {
                // Since the Internal Logger is responsible to log the FastLog.Net internal exceptions , so if in this point we should ignore any unwanted exceptions.
            }



        }



        private void CheckLogFileSize()
        {

            // Gain Lock
            SlimReadWriteLock.GainWriteLock();

            try
            {
                if (string.IsNullOrWhiteSpace(InternalLogFile))
                {
                    return;
                }





                if (!File.Exists(InternalLogFile))
                {
                    // Write the file header after deletion
                    File.AppendAllText(InternalLogFile, FileHeader.GenerateFileHeader(InternalLogFile, "FastLog.Net Internal Logger"));

                    return;
                }



                if (FileHelper.GetFileSizeMB(InternalLogFile) >= InternalExceptionsMaxLogFileSizeMB)
                {
                    File.Delete(InternalLogFile);

                    if (!File.Exists(InternalLogFile))
                    {
                        // Write the file header after deletion
                        File.AppendAllText(InternalLogFile, FileHeader.GenerateFileHeader(InternalLogFile, "FastLog.Net Internal Logger"));
                    }
                }


            }
            catch
            {
                // Since the Internal Logger is responsible to log the FastLog.Net internal exceptions , so if in this point we should ignore any unwanted exceptions.
            }

            finally
            {
                // Release Lock
                SlimReadWriteLock.RelaseWriteLock();
            }

        }

        private void PrintOnConsole(ILogEventModel logToPrint)
        {
            if (_PrintOnConsole)
            {
                if (_PrintOnConsoleOnlyOnDebugMode)
                {
                    if (Debugger.IsAttached)
                    {

                        FluentConsole.Console
                                     .AddLineBreak(1)
                                     .Write("Logger").AddSpace()
                                     .WithBackColor(ConsoleColor.Red)
                                     .Write("Internal Exception")
                                     .ResetColor()
                                     .AddSpace()
                                     .WriteLine("has been occured :")
                                     .WriteLine(useJsonFormat ? logToPrint.ToJsonText() : logToPrint.ToPlainText())
                                     .Print();

                    }
                }
                else
                {

                    FluentConsole.Console
                                 .AddLineBreak(1)
                                 .Write("Logger").AddSpace()
                                 .WithBackColor(ConsoleColor.Red)
                                 .Write("Internal Exception")
                                 .ResetColor()
                                 .AddSpace()
                                 .WriteLine("has been occured :")
                                 .WriteLine(useJsonFormat ? logToPrint.ToJsonText() : logToPrint.ToPlainText())
                                 .Print();
                }
            }
        }



        private void MakeBeepSound()
        {
            // Note : "Beep" only works on Windows® OS.
            // ATTENTION : There's a possibility of "HostProtectionException" or "PlatformNotSupportedException" exception.
            // For more info please visit : https://learn.microsoft.com/en-us/dotnet/api/system.console.beep?view=net-7.0

            try
            {
                if (_Beep)
                {
                    if (_BeepOnlyOnDebugMode)
                    {
                        if (Debugger.IsAttached && RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
                            Console.Beep();
                    }
                    else
                    {
                        // Note : "Beep" only works on Windows® OS.
                        if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows)) Console.Beep();
                    }
                }
            }
            catch
            {
                // Do not catch Beep low level API exceptions.
            }

        }

    }

}

