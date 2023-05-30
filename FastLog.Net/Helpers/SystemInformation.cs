/*---------------------------------------------------------------------------------------------

                ► FastLog.Net , High Performance Logger For .Net ◄



 → Copyright (c) 2020-2023 Shayan Firoozi , Bandar Abbas , Iran , Under MIT License.

 → Contact : <shayan.firoozi@gmail.com>

 → GitHub repository : https://github.com/ShayanFiroozi/FastLog.Net

---------------------------------------------------------------------------------------------*/

using FastLog.Helpers.ExtendedMethods;
using System;
using System.Reflection;

namespace FastLog.Helpers
{

    /// <summary>
    /// Get system information
    /// </summary>
    internal static class SystemInformation
    {
        internal static string GetCurrentAssemblyVersion()
        {
            try
            {
                return Assembly.GetExecutingAssembly()?.GetName()?.Version != null ?
                       Assembly.GetExecutingAssembly()?.GetName()?.Version?.ToString() :
                       "N/A";
            }
            catch
            {
                return "N/A";
            }
        }

        internal static string GetMachineName()
        {
            try
            {
                return !string.IsNullOrWhiteSpace(Environment.MachineName)
                    ? Environment.MachineName
                    : "N/A";
            }
            catch
            {
                return "N/A";
            }
        }



        internal static string GetCurrentUserName()
        {
            try
            {
                return !string.IsNullOrWhiteSpace(Environment.UserName)
                    ? Environment.UserName
                    : "N/A";
            }
            catch
            {
                return "N/A";
            }
        }



        internal static string GetCurrentAssemblyBuildDate()
        {
            try
            {


                return Assembly.GetExecutingAssembly()?.GetName()?.Version != null
                ? new DateTime(2000, 1, 1).AddDays(Assembly.GetExecutingAssembly().GetName().Version.Build).ToFriendlyDate()
                : "N/A";
            }
            catch
            {
                return "N/A";
            }
        }


        internal static string GetOSInfo()
        {
            try
            {
                return Environment.OSVersion != null
                ? Environment.OSVersion?.ToString()
                : "N/A";
            }
            catch
            {
                return "N/A";
            }
        }


        internal static string GetDotNetRuntime()
        {
            try
            {
                if (AppDomain.CurrentDomain.SetupInformation.TargetFrameworkName != null)
                {
                    return string.IsNullOrWhiteSpace(AppDomain.CurrentDomain?.SetupInformation?.TargetFrameworkName) ? "N/A" :
                            AppDomain.CurrentDomain?.SetupInformation?.TargetFrameworkName;
                }
                else
                {
                    return "N/A";
                }
            }
            catch
            {
                return "N/A";
            }
        }



    }
}
