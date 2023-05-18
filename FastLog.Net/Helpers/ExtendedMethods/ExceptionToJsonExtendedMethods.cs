using FastLog.Enums;
using System;
using System.Text;
using TrendSoft.FastLog.Models;

namespace FastLog.Net.Helpers.ExtendedMethods
{
    internal static class ExceptionToJsonExtendedMethods
    {

        public static string ToJsonText(this Exception exception)
        {
            if (exception is null)
            {
                return "N/A";
            }

            StringBuilder finalMessage = new StringBuilder();

            _ = finalMessage.Append("[\n     {").Append('\n')

                            .Append($"      \"Id\": \"{exception.HResult}\"")
                             .Append(',').Append('\n')

                            .Append($"      \"Message\": \"{exception.Message}\"")
                            .Append(',').Append('\n')

                            .Append($"      \"Inner Exception\": \"{(exception.InnerException == null ? "N/A" : $"\n{exception.InnerException.ToJsonText()}\n")}\"")
                            .Append(',').Append('\n')

                            .Append($"      \"Source\": \"{(string.IsNullOrWhiteSpace(exception.Source) ? "N/A" : exception.Source)}\"")
                            .Append(',').Append('\n')

                            .Append($"      \"StackTrace\": \"{(string.IsNullOrWhiteSpace(exception.StackTrace) ? "N/A" : exception.StackTrace)}\"")
                            .Append(',').Append('\n')

                            .Append($"      \"Target Site\": \"{(exception.TargetSite == null ? "N/A" : exception.TargetSite.Name)}\"")
                            .Append(',').Append('\n');
        
            _ = finalMessage.Append("     }\n   ]");


            return finalMessage.ToString();

        }


    }
}

