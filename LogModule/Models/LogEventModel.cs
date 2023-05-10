using System;
using System.Text;

namespace TrendSoft.LogModule.Models
{
    public class LogEventModel : IDisposable
    {
        private bool disposedValue;

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
            : this(LogEventModel.LogTypeEnum.EXCEPTION,
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


        #region Dispose Pattern
        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    // TODO: dispose managed state (managed objects)

                    Source = null;
                    LogText = null;
                    ExtraInfo = null;
                }

                // TODO: free unmanaged resources (unmanaged objects) and override finalizer
                // TODO: set large fields to null
                disposedValue = true;
            }
        }

        // // TODO: override finalizer only if 'Dispose(bool disposing)' has code to free unmanaged resources
        // ~LogMessageModel()
        // {
        //     // Do not change this code. Put cleanup code in 'Dispose(bool disposing)' method
        //     Dispose(disposing: false);
        // }

        public void Dispose()
        {
            // Do not change this code. Put cleanup code in 'Dispose(bool disposing)' method
            Dispose(disposing: true);
            GC.SuppressFinalize(this);
        }
        #endregion


    }
}
