using System;
using System.Text;

namespace LogModule
{
    public class LogMessage
    {

        #region Constructors
        public LogMessage(LogTypeEnum LogType,
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

        // It's neccessary for LiteDB
        public LogMessage()
        {

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

        public DateTime DateTime { get; set; }


        public LogTypeEnum LogType { get; set; }



        public string Source { get; set; }



        public string LogText { get; set; }



        public string ExtraInfo { get; set; }





        #endregion


        #region Methods
        public StringBuilder GetLogMessage()
        {
            StringBuilder finalMessage = new();

            _ = finalMessage.Append(DateTime.ToString("yyyy/MM/dd HH:mm:ss"));
            _ = finalMessage.Append(' ');

            _ = finalMessage.Append('(');
            _ = finalMessage.Append(LogType.ToString());
            _ = finalMessage.Append(')');
            _ = finalMessage.Append(" -> ");

            _ = finalMessage.Append(LogText);


            if (!string.IsNullOrWhiteSpace(ExtraInfo))
            {
                if (LogType != LogTypeEnum.EXCEPTION)
                {
                    _ = finalMessage.Append(" , Details : ");
                }

                _ = finalMessage.Append(ExtraInfo);
            }

            if (!string.IsNullOrWhiteSpace(Source))
            {
                _ = finalMessage.Append(" , Source : ");
                _ = finalMessage.Append(Source);
            }


            _ = finalMessage.Append(Environment.NewLine);



            return finalMessage;

        }
        #endregion


    }
}
