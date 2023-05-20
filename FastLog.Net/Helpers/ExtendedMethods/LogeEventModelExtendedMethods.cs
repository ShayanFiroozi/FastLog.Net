using FastLog.Enums;
using System;
using System.Text;
using TrendSoft.FastLog.Models;

namespace FastLog.Helpers.ExtendedMethods
{
    internal static class LogeEventModelExtendedMethods
    {

        public static string ToPlainText(this LogEventModel logEventModel)
        {
            StringBuilder finalMessage = new StringBuilder();

            _ = finalMessage.Append('[')
                            .Append(logEventModel.LogEventType.ToString())
                            .Append(']')
                            .Append(logEventModel.EventId != 0 ? $" [{logEventModel.EventId}]" : string.Empty)
                            .Append(" -> ")
                            .Append(logEventModel.EventMessage);


            if (!string.IsNullOrWhiteSpace(logEventModel.Details))
            {
                if (logEventModel.LogEventType != LogEventTypes.EXCEPTION)
                {
                    _ = finalMessage.Append(" , Details: ");
                }

                _ = finalMessage.Append(logEventModel.Details);
            }


            if (logEventModel.LogMachineName)
            {
                if (logEventModel.LogEventType != LogEventTypes.EXCEPTION)
                {
                    _ = finalMessage.Append($" , MachineName: \"{Environment.MachineName}\"");
                }
                else
                {
                    _ = finalMessage.Append($"\nMachineName: \"{Environment.MachineName}\"");
                }
            }


            if (!string.IsNullOrWhiteSpace(logEventModel.ApplicationName))
            {
                if (logEventModel.LogEventType != LogEventTypes.EXCEPTION)
                {
                    _ = finalMessage.Append($" , App Name: \"{logEventModel.ApplicationName}\"");
                }
                else
                {
                    _ = finalMessage.Append($"\nApp Name: \"{logEventModel.ApplicationName}\"");
                }

            }



            if (logEventModel.LogEventType != LogEventTypes.EXCEPTION)
            {
                _ = finalMessage.Append($" , DateTime: \"{logEventModel.DateTime.ToFriendlyDateTime()}\"");
            }
            else
            {
                _ = finalMessage.Append($"\nDateTime: \"{logEventModel.DateTime.ToFriendlyDateTime()}\"");
            }


            _ = finalMessage.Append(Environment.NewLine);




            return finalMessage.ToString();

        }




        public static string ToJsonText(this LogEventModel logEventModel)
        {
            StringBuilder finalMessage = new StringBuilder();

            _ = finalMessage.Append("{").Append('\n')


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
                         .Append(',').Append('\n');

            }

            if (logEventModel.LogMachineName)
            {
                _ = finalMessage.Append($" \"MachineName\": \"{Environment.MachineName}\"")
                    .Append(',').Append('\n');
            }


            if (!string.IsNullOrWhiteSpace(logEventModel.ApplicationName))
            {
                _ = finalMessage.Append($" \"Application\": \"{(string.IsNullOrWhiteSpace(logEventModel.ApplicationName) ? "N/A" : logEventModel.ApplicationName)}\"")
                    .Append('\n');
            }

            string finalResult = finalMessage.ToString();

            // Trim the last ',' character if exists !

            if (finalResult.Substring(finalResult.Length - 1, 1) == ",") finalResult = finalResult.Substring(0, finalResult.Length - 1);


            finalResult += $"}},\n";
                            


            return finalResult;

        }


    }
}

