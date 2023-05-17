using FastLog.Enums;
using System;
using System.Text;
using TrendSoft.FastLog.Models;

namespace FastLog.Net.Helpers.ExtendedMethods
{
    internal static class LogeEventModelExtendedMethods
    {

        public static string ToLogMessage(this LogEventModel logEventModel)
        {
            StringBuilder finalMessage = new StringBuilder();

            _ = finalMessage.Append('[')
                            .Append(logEventModel.LogEventType.ToString())
                            .Append(']')
                            .Append(logEventModel.EventId != 0 ? $" [{logEventModel.EventId}]" : string.Empty)
                            .Append(" -> ")
                            .Append(logEventModel.EventText);


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
            //_ = finalMessage.Append(Environment.NewLine);



            return finalMessage.ToString();

        }


    }
}

