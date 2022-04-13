using LogModule;
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
        public void FileLogger_Throw_Exception_On_Invalid_Path()
        {
            Assert.Throws(new ArgumentNullException().GetType(), () => { new FileLogger(" ", "LogModuleTest.log"); });

        }

        [Test]
        public void FileLogger_Throw_Exception_On_Invalid_FileName()
        {
            Assert.Throws(new ArgumentNullException().GetType(), () => { new FileLogger("LogModuleTest", " "); });

        }




        [Test]
        public void FileLogger_SaveLog_Method_Throws_Exception_On_Null_Parameter()
        {

            FileLogger_Constructor_Test();

            Assert.Throws(new ArgumentNullException().GetType(), () => { fileLogger.SaveLog(null); });

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