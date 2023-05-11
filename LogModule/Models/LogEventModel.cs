using System;
using System.Text;

namespace TrendSoft.LogModule.Models
{
    public class LogEventModel
    {

        #region Constructors

        public LogEventModel(LogTypeEnum LogType,
                          string LogText,
                          string ExtraInfo = "",
                          string Source = "")
        {
            DateTime = DateTime.Now;
            this.LogType = LogType;
            this.LogText = LogText;
            this.ExtraInfo = ExtraInfo;
            this.Source = Source;
        }


        public LogEventModel(Exception exception)
            : this(LogTypeEnum.EXCEPTION,
                   " Message : " + exception.Message ?? "-",
                   " InnerMessage : " + (exception.InnerException?.Message ?? "-") +
                   " , " +
                   " StackTrace : " + (exception.StackTrace ?? "-"),
                   (exception.Source ?? "-"))
        {
        }



        public LogEventModel()
        {
            // This default constructor is neccessary for LiteDB.
        }

        #endregion


        #region Enumerations

        public enum LogTypeEnum : byte
        {
            INFO = 0,
            WARNING = 1,
            ERROR = 2,
            EXCEPTION = 3,
            DEBUG = 4

        }

        #endregion



        #region Properties

        public DateTime DateTime { get; private set; }


        public LogTypeEnum LogType { get; private set; }



        public string Source { get; private set; }



        public string LogText { get; private set; }



        public string ExtraInfo { get; private set; }





        #endregion


        public override string ToString()
        {
            return GetLogMessage().ToString();
        }


        private StringBuilder GetLogMessage()
        {
            StringBuilder finalMessage = new();

            finalMessage.Append(DateTime.ToString("yyyy/MM/dd HH:mm:ss"))
                        .Append(' ')
                        .Append('[')
                        .Append(LogType.ToString())
                        .Append(']')
                        .Append(" -> ")
                        .Append(LogText);


            if (!string.IsNullOrWhiteSpace(ExtraInfo))
            {
                if (LogType is not LogTypeEnum.EXCEPTION)
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


            _ = finalMessage.Append(Environment.NewLine);



            return finalMessage;

        }


   

    }
}
