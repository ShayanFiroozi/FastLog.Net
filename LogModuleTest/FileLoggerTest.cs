using LogModule.FileLogger;
using NUnit.Framework;
using System;

namespace LogModuleTest
{
    public class FileLoggerTest
    {

       
        public FileLogger fileLogger;


        [Test]
        public void FileLogger_Constructor_Test()
        {
            fileLogger = new FileLogger("LogModuleTest", "LogModuleTest.log");

        }



      



        [Test]
        public void FileLogger_SaveLog_Test()
        {
            FileLogger_Constructor_Test();

            LogMessageTest logMessageTest = new();

            logMessageTest.Setup();

           
                fileLogger.SaveLog(logMessageTest.logMessage);

        }



        [Test]
        public void FileLogger_Delete_Log_File()
        {
            FileLogger_Constructor_Test();

            fileLogger.DeleteLogFile();

        }




    }
}