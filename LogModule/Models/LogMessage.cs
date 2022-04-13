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
            this.DateTime = DateTime.Now;
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

            _ = finalMessage.Append(this.DateTime.ToString("yyyy/MM/dd HH:mm:ss"));
            _ = finalMessage.Append(" ");
            _ = finalMessage.Append(this.LogType.ToString());
            _ = finalMessage.Append(" -> ");
            _ = finalMessage.Append(this.Source);
            _ = finalMessage.Append(" , ");
            _ = finalMessage.Append(this.LogText);
            if (!string.IsNullOrWhiteSpace(this.ExtraInfo))
            {
                _ = finalMessage.Append(" , ");
                _ = finalMessage.Append(this.ExtraInfo);
            }

            return finalMessage;

        }
        #endregion


    }
}
