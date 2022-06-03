namespace LogModule
{


    public interface IFileLogger : ILoggerAgent
    {

        public void SaveLog(LogMessage logMessage, bool threadSafeWrite);



    }
}
