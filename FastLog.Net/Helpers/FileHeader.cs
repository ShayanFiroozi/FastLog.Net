/*---------------------------------------------------------------------------------------------

                ► FastLog.Net , High Performance Logger For .Net ◄



 → Copyright (c) 2020-2023 Shayan Firoozi , Bandar Abbas , Iran , Under MIT License.

 → Contact : <shayan.firoozi@gmail.com>

 → GitHub repository : https://github.com/ShayanFiroozi/FastLog.Net

---------------------------------------------------------------------------------------------*/

using FastLog.Helpers.ExtendedMethods;
using System;

namespace FastLog.Helpers
{

    /// <summary>
    /// Generate file header of the log file which contains some information about current system and logger info.
    /// </summary>
    internal static class FileHeader
    {
        /* 

         *-->  Output example of the File header generator : 

                 --------------------------------------------------------------------------------

               ► FastLog.Net Version: "1.0.8543.39932" (64-bit) , Build: "2023-05-23" ◄

               → Log File: "D:\Logs\InternalEventsLog.log"
               → Creation DateTime: "2023-05-23 22:11:13"

               → Machine Name: "SHAYAN-PC"
               → OS: "Microsoft Windows NT 10.0.19045.0 (64-bit)"
               → .NET Runtime Version: ".NETCoreApp,Version=v7.0"

               → Logger Name: "FastLog.Net Internal Logger"
               → Current User: "Shayan"

               → Source Code: https://github.com/ShayanFiroozi/FastLog.Net
               → Nuget Package: https://www.nuget.org/packages/FastLog.Net/

               --------------------------------------------------------------------------------
        */

        internal static string GenerateFileHeader(string logFile, string LoggerName = "N/A")
        {
            try
            {

                return $"{new string('-', 80)}\n\n" +
                       $"► FastLog.Net Version: " +
                       $"\"{SystemInformation.GetCurrentAssemblyVersion()}\" ({(Environment.Is64BitProcess ? "64-bit" : "32-bit")})" +
                       $" , Build: \"{SystemInformation.GetCurrentAssemblyBuildDate()}" +
                       $"\" ◄\n\n" +

                       $"→ Log File: \"{logFile}\"\n" +
                       $"→ Creation DateTime: \"{DateTime.Now.ToFriendlyDateTime()}\"\n\n" +
                       $"→ Machine Name: \"{SystemInformation.GetMachineName()}\"\n" +

                       $"→ OS: \"{SystemInformation.GetOSInfo()}" +
                       $" ({(Environment.Is64BitOperatingSystem ? "64-bit" : "32-bit")})\"\n" +

                       $"→ .NET Runtime Version: \"{SystemInformation.GetDotNetRuntime()}\"\n\n" +


                       $"→ Logger Name: \"{(!string.IsNullOrWhiteSpace(LoggerName) ? LoggerName : "N/A")}\"\n" +
                       $"→ Current User: \"{SystemInformation.GetCurrentUserName()}\"\n\n" +

                       $"→ Source Code: https://github.com/ShayanFiroozi/FastLog.Net\n" +
                        $"→ Nuget Package: https://www.nuget.org/packages/FastLog.Net\n\n" +

                       $"{new string('-', 80)}\n\n";
            }
            catch (Exception ex)
            {
                return $"Failed to create the Log File Header!\n{ex.ToJsonText()}{new string('-', 80)}\n\n";
            }

        }



    }
}
