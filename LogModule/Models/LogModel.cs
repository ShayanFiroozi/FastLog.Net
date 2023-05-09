using System;
using System.Text;

namespace TrendSoft.LogModule.Models
{
    public class LogMessageModel
    {

        #region Constructors

        public LogMessageModel(LogTypeEnum LogType,
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


        public LogMessageModel()
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
