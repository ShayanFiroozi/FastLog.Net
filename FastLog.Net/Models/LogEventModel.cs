using FastLog.Net.Enums;
using System;
using System.Text;

namespace TrendSoft.FastLog.Models
{
    public class LogEventModel
    {

        #region Constructors

        public LogEventModel(LogEventTypes LogEventType,
                          string LogText,
                          string ExtraInfo = "",
                          string Source = "",
                          bool LogMachineName = false)
        {
            DateTime = DateTime.Now;
            this.LogEventType = LogEventType;
            this.LogText = LogText;
            this.ExtraInfo = ExtraInfo;
            this.Source = Source;
            this.LogMachineName = LogMachineName;
        }


        public LogEventModel(Exception exception, bool LogMachineName = false)
            : this(LogEventTypes.EXCEPTION,
                   " Message : " + exception.Message ?? "-",
                   " InnerMessage : " + (exception.InnerException?.Message ?? "-") +
                   " , " +
                   " StackTrace : " + (exception.StackTrace ?? "-"),
                   exception.Source ?? "-",
                   LogMachineName)
        {
        }



        public LogEventModel()
        {
            // This default constructor is neccessary for LiteDB.
        }

        #endregion


     


        #region Properties

        public DateTime DateTime { get; private set; }


        public LogEventTypes LogEventType { get; private set; }



        public string Source { get; private set; }



        public string LogText { get; private set; }



        public string ExtraInfo { get; private set; }


        public bool LogMachineName { get; private set; }





        #endregion



        public string GetLogMessage(bool DateTimeIncluded)
        {
            StringBuilder finalMessage = new StringBuilder();

            if (DateTimeIncluded)
            {
                _ = finalMessage.Append(DateTime.ToString("yyyy/MM/dd HH:mm:ss"));
            }

            _ = finalMessage.Append(' ')
                            .Append('[')
                            .Append(LogEventType.ToString())
                            .Append(']')
                            .Append(" -> ")
                            .Append(LogText);


            if (!string.IsNullOrWhiteSpace(ExtraInfo))
            {
                if (LogEventType != LogEventTypes.EXCEPTION)
                {
                    _ = finalMessage.Append(" , Details : ");
                }

                _ = finalMessage.Append(ExtraInfo);
            }

            if (!string.IsNullOrWhiteSpace(Source))
            {
                _ = finalMessage.Append(" , Source : ")
                                .Append(Source);
            }

            if (LogMachineName)
            {
                _ = finalMessage.Append($" , MachineName : {Environment.MachineName}");
            }

            _ = finalMessage.Append(Environment.NewLine);



            return finalMessage.ToString();

        }




    }
}
