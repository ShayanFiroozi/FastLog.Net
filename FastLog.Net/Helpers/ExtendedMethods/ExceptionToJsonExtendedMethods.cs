/*---------------------------------------------------------------------------------------------

                ► FastLog.Net , High Performance Logger For .Net ◄



 → Copyright (c) 2020-2023 Shayan Firoozi , Bandar Abbas , Iran , Under MIT License.

 → Contact : <shayan.firoozi@gmail.com>

 → GitHub repository : https://github.com/ShayanFiroozi/FastLog.Net

---------------------------------------------------------------------------------------------*/

using System;
using System.Text;

namespace FastLog.Helpers.ExtendedMethods
{

    /// <summary>
    /// Generate Json text from "Exception" class.
    /// </summary>
    internal static class ExceptionToJsonExtendedMethods
    {

        public static string ToJsonText(this Exception exception, int SpaceLevel = 1)
        {
            if (exception is null)
            {
                return "N/A";
            }

            int spaceCount = SpaceLevel * 4;

            StringBuilder finalMessage = new StringBuilder();

            _ = finalMessage.Append($"{new string(' ', spaceCount)}{{").Append('\n')

                            .Append($"{new string(' ', spaceCount + 2)}\"Id\": \"{exception.HResult}\"")
                             .Append(',').Append('\n')

                            .Append($"{new string(' ', spaceCount + 2)}\"Message\": \"{exception.Message}\"")
                            .Append(',').Append('\n')

                            .Append($"{new string(' ', spaceCount + 2)}\"Inner Exception\": {(exception.InnerException == null ? "\"N/A\"" : $"\n{exception.InnerException.ToJsonText(SpaceLevel + 1)}")}")
                            .Append(',').Append('\n')

                            .Append($"{new string(' ', spaceCount + 2)}\"Source\": \"{(string.IsNullOrWhiteSpace(exception.Source) ? "N/A" : exception.Source)}\"")
                            .Append(',').Append('\n')

                            .Append($"{new string(' ', spaceCount + 2)}\"StackTrace\": \"{(string.IsNullOrWhiteSpace(exception.StackTrace) ? "N/A" : exception.StackTrace)}\"")
                            .Append(',').Append('\n')

                            .Append($"{new string(' ', spaceCount + 2)}\"Target Site\": \"{(exception.TargetSite == null ? "N/A" : exception.TargetSite.Name)}\"")
                            .Append($"\n{new string(' ', spaceCount)}}}");

            //_ = finalMessage.Append("}\n");


            return finalMessage.ToString();

        }


    }
}

