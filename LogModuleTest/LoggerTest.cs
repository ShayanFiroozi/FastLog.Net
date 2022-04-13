using LogModule;
using NUnit.Framework;

namespace LogModuleTest
{
    public class LoggerTest
    {

        [Test]
        public void LoggerExecuationTest()
        {

            Logger logger = new();
            
            
            LogMessageTest logMessage = new(); // init a message sample
            FileLoggerTest fileLogger = new(); // init the file logger

            fileLogger.FileLogger_Constructor_Test(); // create the filelogger
            logMessage.Setup(); // create the message


            logger.RegisterLoggingChannel(fileLogger.fileLogger); // register filelogger in loggerchannels

            logger.ExecuteLogging(logMessage.logMessage); // Executing the logger channel


        }


       



    }
}