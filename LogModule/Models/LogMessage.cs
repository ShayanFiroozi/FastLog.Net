using System;
using System.Text;

namespace LogModule
{
    public sealed class LogMessage
    {

        #region Constructors
        public LogMessage(LogTypeEnum LogType,
                          string Source,
                          string LogText,
                          string ExtraInfo = "")
        {
            DateTime = DateTime.Now;
            this.LogType = LogType;
            this.Source = Source;
            this.LogText = LogText;
            this.ExtraInfo = ExtraInfo;
        }

        #endregion


        #region Enumerations


        public enum LogTypeEnum : byte
        {
            INFO = 0,
            WARNING = 1,
            ERROR = 2
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
            _ = finalMessage.Append(" ");
            _ = finalMessage.Append(LogType.ToString());
            _ = finalMessage.Append(" -> ");
            _ = finalMessage.Append(Source);
            _ = finalMessage.Append(" , ");
            _ = finalMessage.Append(LogText);
            if (!string.IsNullOrWhiteSpace(ExtraInfo))
            {
                _ = finalMessage.Append(" , ");
                _ = finalMessage.Append(ExtraInfo);
            }

            return finalMessage;

        }
        #endregion


    }
}
