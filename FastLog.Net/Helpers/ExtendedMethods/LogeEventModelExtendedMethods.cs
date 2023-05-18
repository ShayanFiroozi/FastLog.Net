using FastLog.Enums;
using System;
using System.Text;
using TrendSoft.FastLog.Models;

namespace FastLog.Net.Helpers.ExtendedMethods
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
                _ = finalMessage.Append($" , DateTime: \"{logEventModel.DateTime.ToLogFriendlyDateTime()}\"");
            }
            else
            {
                _ = finalMessage.Append($"\nDateTime: \"{logEventModel.DateTime.ToLogFriendlyDateTime()}\"");
            }


            _ = finalMessage.Append(Environment.NewLine);




            return finalMessage.ToString();

        }




        public static string ToJsonText(this LogEventModel logEventModel)
        {
            StringBuilder finalMessage = new StringBuilder();

            _ = finalMessage.Append("{").Append('\n')


                            .Append($" \"DateTime\": \"{logEventModel.DateTime.ToLogFriendlyDateTime()}\"")
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

            _ = finalMessage.Append("},")
                            .Append('\n');

  
            return finalMessage.ToString();

        }


    }
}

