using LogModule.Agents;
using NUnit.Framework;

namespace LogModuleTest
{
    public class FileLoggerTest
    {


        public FileLogger fileLogger;


        [Test]
        public void FileLogger_Constructor_Test()
        {
            fileLogger = new FileLogger("D:\\LogsTest\\LogModuleTest.log",10);

        }







        [Test]
        public void FileLogger_SaveLog_Test()
        {
            FileLogger_Constructor_Test();

            LogMessageTest logMessageTest = new();

            logMessageTest.Setup();


            fileLogger.SaveLog(logMessage: logMessageTest.logMessage, threadSafeWrite: true);

        }



        [Test]
        public void FileLogger_Delete_Log_File()
        {
            FileLogger_Constructor_Test();

            fileLogger.DeleteLogFile();

        }




    }
}