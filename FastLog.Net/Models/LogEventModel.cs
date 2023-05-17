using FastLog.Net.Enums;
using System;
using System.Text;

namespace TrendSoft.FastLog.Models
{
    public class LogEventModel
    {

        #region Constructors

        public LogEventModel(LogEventTypes LogEventType,
                             string EventText,
                             string Details = "",
                             bool LogMachineName = false,
                             string ApplicationName = "",
                             int EventId = 0)
        {
            DateTime = DateTime.Now;
            this.LogEventType = LogEventType;
            this.EventText = EventText;
            this.Details = Details;
            this.LogMachineName = LogMachineName;
            this.ApplicationName = ApplicationName;
            this.EventId = EventId;
        }


        public LogEventModel(Exception exception, bool LogMachineName = false, string ApplicationName = "", int EventId = 0)
            : this(LogEventTypes.EXCEPTION,

                   $"\nId : {exception.HResult}\n" +
                   $"Message : {exception.Message ?? "N/A"}\n",

                   $"InnerException : {exception.InnerException?.Message ?? "N/A"}\n" +
                   $"StackTrace : {exception.StackTrace ?? "N/A"}\n" +
                   $"Source : {exception.Source ?? "N/A"}\n" +
                   $"Target Site : {(exception.TargetSite != null ? exception.TargetSite.Name : "N/A")}",

                   LogMachineName,
                   ApplicationName,
                   EventId)
        { }



        public LogEventModel()
        {
            // This default constructor is neccessary for LiteDB.
        }

        #endregion


        #region Properties


        public int EventId { get; private set; } = 0;

        public DateTime DateTime { get; private set; }

        public LogEventTypes LogEventType { get; private set; }

        public string EventText { get; private set; }

        public string Details { get; private set; }

        public bool LogMachineName { get; private set; }

        public string ApplicationName { get; private set; }


        #endregion



        public string GetLogMessage(bool DateTimeIncluded)
        {
            StringBuilder finalMessage = new StringBuilder();

            _ = finalMessage.Append('[')
                            .Append(LogEventType.ToString())
                            .Append(']')
                            .Append(EventId != 0 ? $" [{EventId}]":string.Empty)
                            .Append(" -> ")
                            .Append(EventText);


            if (!string.IsNullOrWhiteSpace(Details))
            {
                if (LogEventType != LogEventTypes.EXCEPTION)
                {
                    _ = finalMessage.Append(" , Details: ");
                }

                _ = finalMessage.Append(Details);
            }


            if (LogMachineName)
            {
                if (LogEventType != LogEventTypes.EXCEPTION)
                {
                    _ = finalMessage.Append($" , MachineName: \"{Environment.MachineName}\"");
                }
                else
                {
                    _ = finalMessage.Append($"\nMachineName: \"{Environment.MachineName}\"");
                }
            }


            if (!string.IsNullOrWhiteSpace(ApplicationName))
            {
                if (LogEventType != LogEventTypes.EXCEPTION)
                {
                    _ = finalMessage.Append($" , App Name: \"{ApplicationName}\"");
                }
                else
                {
                    _ = finalMessage.Append($"\nApp Name: \"{ApplicationName}\"");
                }

            }


            if (DateTimeIncluded)
            {
                if (LogEventType != LogEventTypes.EXCEPTION)
                {
                    _ = finalMessage.Append($" , DateTime: \"{DateTime.ToString("yyyy/MM/dd HH:mm:ss")}\"");
                }
                else
                {
                    _ = finalMessage.Append($"\nDateTime: \"{DateTime.ToString("yyyy/MM/dd HH:mm:ss")}\"");
                }
            }

            _ = finalMessage.Append(Environment.NewLine);
            //_ = finalMessage.Append(Environment.NewLine);



            return finalMessage.ToString();

        }




    }
}
