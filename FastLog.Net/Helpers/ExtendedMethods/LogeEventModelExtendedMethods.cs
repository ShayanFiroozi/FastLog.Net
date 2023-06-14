/*---------------------------------------------------------------------------------------------

                ► FastLog.Net , High Performance Logger For .Net ◄



 → Copyright (c) 2020-2023 Shayan Firoozi , Bandar Abbas , Iran , Under MIT License.

 → Contact : <shayan.firoozi@gmail.com>

 → GitHub repository : https://github.com/ShayanFiroozi/FastLog.Net

---------------------------------------------------------------------------------------------*/

using FastLog.Enums;
using FastLog.Interfaces;
using System;
using System.Text;

namespace FastLog.Helpers.ExtendedMethods
{

    /// <summary>
    /// Convert "LogeEventModel" to the "Plain" or "Json" text.
    /// </summary>
    internal static class LogeEventModelExtendedMethods
    {

        public static string ToPlainText(this ILogEventModel logEventModel, bool IncludeEventType = true, bool IncludeDateTime = true)
        {
            StringBuilder finalMessage = new StringBuilder();

            if (IncludeEventType)
            {
                _ = finalMessage.Append('[')
                                .Append(logEventModel.LogEventType.ToString())
                                .Append(']')
                                .Append(logEventModel.EventId != 0 ? $" [{logEventModel.EventId}]" : string.Empty)
                                .Append(" → ");

            }

            _ = finalMessage.Append(logEventModel.EventMessage);


            if (!string.IsNullOrWhiteSpace(logEventModel.Details))
            {
                if (logEventModel.LogEventType != LogEventTypes.EXCEPTION)
                {
                    _ = finalMessage.Append(" , Details: ");
                }

                _ = finalMessage.Append(logEventModel.Details);
            }



            if (IncludeDateTime)
            {

                if (logEventModel.LogEventType != LogEventTypes.EXCEPTION)
                {
                    _ = finalMessage.Append($" , DateTime: \"{logEventModel.DateTime.ToFriendlyDateTime()}\"");
                }
                else
                {
                    _ = finalMessage.Append($"\nDateTime: \"{logEventModel.DateTime.ToFriendlyDateTime()}\"");
                }

            }


            _ = finalMessage.Append(Environment.NewLine);




            return finalMessage.ToString();

        }




        public static string ToJsonText(this ILogEventModel logEventModel)
        {
            StringBuilder finalMessage = new StringBuilder();

            _ = finalMessage.Append('{').Append('\n')


                            .Append($" \"DateTime\": \"{logEventModel.DateTime.ToFriendlyDateTime()}\"")
                             .Append(',').Append('\n')

                            .Append($" \"Type\": \"{logEventModel.LogEventType}\"")
                             .Append(',').Append('\n')

                            .Append($" \"Id\": \"{logEventModel.EventId}\"")
                             .Append(',').Append('\n');


            if (logEventModel.LogEventType == LogEventTypes.EXCEPTION)
            {
                _ = finalMessage.Append($" \"Message\": \n{logEventModel.Exception.ToJsonText()}")
                                 .Append(',').Append('\n');
            }
            else
            {

                _ = finalMessage.Append($" \"Message\": \"{(string.IsNullOrWhiteSpace(logEventModel.EventMessage) ? "N/A" : logEventModel.EventMessage)}\"")
                                 .Append(',').Append('\n');


                _ = finalMessage.Append($" \"Details\": \"{(string.IsNullOrWhiteSpace(logEventModel.Details) ? "N/A" : logEventModel.Details)}\"")
                         .Append(',');

            }


            string finalResult = finalMessage.ToString();

            // Trim the last ',' character if exists !

            if (finalResult.Substring(finalResult.Length - 1, 1) == ",")
            {
                finalResult = finalResult.Substring(0, finalResult.Length - 1);
            }


            finalResult += $"\n}},\n";



            return finalResult;

        }


    }
}

