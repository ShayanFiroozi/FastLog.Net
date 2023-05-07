namespace LogModule
{


    public interface IFileLogger : ILoggerAgent
    {

        public void SaveLog(LogModel logMessage);



    }
}
