using FastLog.Helpers.ExtendedMethods;
using System;
using System.Reflection;

namespace FastLog.Helpers
{
    internal static class FileHeader
    {
        internal static string GenerateFileHeader(string logFile , string ApplicationName = "N/A")
        {
            try
            {

                return $"{new string(' ', 0)} FastLog.Net Version:" +
                       $"\"{Assembly.GetExecutingAssembly().GetName().Version}\"" +
                       $" , Build: \"{Assembly.GetExecutingAssembly().GetName().Version}({(Environment.Is64BitProcess ? "64-bit" : "32-bit")})\"{new string(' ', 20)}\n\n" +

                       $"{new string(' ', 0)}-> Log File: \"{logFile}\"\n" +
                       $"{new string(' ', 0)}-> Creation DateTime: \"{DateTime.Now.ToLogFriendlyDateTime()}\"\n" +
                       $"{new string(' ', 0)}-> Machine Name: \"{Environment.MachineName}\"{new string(' ', 20)}\n" +

                       $"{new string(' ', 0)}-> OS Name: \"{Environment.OSVersion}" +
                       $"({(Environment.Is64BitOperatingSystem ? "64-bit" : "32-bit")})\"{new string(' ', 20)}\n" +

                       $"{new string(' ', 0)}-> Application Name: \"{(string.IsNullOrWhiteSpace(ApplicationName) ? "N/A" : ApplicationName)}\"{new string(' ', 20)}\n" +
                       $"{new string(' ', 0)}-> Current User: \"{Environment.UserName}\"{new string(' ', 20)}\n\n" +

                       $"{new string(' ', 0)}-> Source Code: https://github.com/ShayanFiroozi/FastLog.Net{new string(' ', 20)}\n\n" +
                       $"{new string('-', 80)}\n\n";
            }
            catch(Exception ex)
            {
                return $"Failed to create the Log File Header!\n{ex.ToJsonText()}{new string('-', 80)}\n\n";
            }

        }



    }
}
