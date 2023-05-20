using FastLog.Helpers.ExtendedMethods;
using System;

namespace FastLog.Helpers
{
    internal static class FileHeader
    {
        internal static string GenerateFileHeader(string logFile , string ApplicationName = "N/A")
        {
            try
            {

                return $"► FastLog.Net Version: " +
                       $"\"{SystemInformation.GetCurrentAssemblyVersion()}\" ({(Environment.Is64BitProcess ? "64-bit" : "32-bit")})" +
                       $" , Build: \"{SystemInformation.GetCurrentAssemblyBuildDate()}" +
                       $"\" ◄\n\n" +

                       $"-> Log File: \"{logFile}\"\n" +
                       $"-> Creation DateTime: \"{DateTime.Now.ToFriendlyDateTime()}\"\n\n" +
                       $"-> Machine Name: \"{SystemInformation.GetMachineName()}\"\n" +

                       $"-> OS: \"{SystemInformation.GetOSInfo()}" +
                       $" ({(Environment.Is64BitOperatingSystem ? "64-bit" : "32-bit")})\"\n" +
                       
                       $"-> .NET Runtime Version: \"{SystemInformation.GetDotNetRuntime()}\"\n\n" +


                       $"-> Application Name: \"{(!string.IsNullOrWhiteSpace(ApplicationName) ? ApplicationName : "N/A")}\"\n" +
                       $"-> Current User: \"{SystemInformation.GetCurrentUserName()}\"\n\n" +

                       $"-> Source Code: https://github.com/ShayanFiroozi/FastLog.Net\n\n" +
                       $"{new string('-', 120)}\n\n";
            }
            catch(Exception ex)
            {
                return $"Failed to create the Log File Header!\n{ex.ToJsonText()}{new string('-', 80)}\n\n";
            }

        }



    }
}
