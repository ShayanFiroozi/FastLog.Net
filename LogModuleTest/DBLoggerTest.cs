using LogModule.Agents;
using NUnit.Framework;

namespace LogModuleTest
{
    public class DBLoggerTest
    {


        public DBLogger dbLogger;


        [Test]
        public void DBLogger_Constructor_Test()
        {
            dbLogger = new DBLogger("D:\\LogsTest\\LogModuleTest.dataB", 10);

        }



        [Test]
        public void DBLogger_SaveLog_Test()
        {
            DBLogger_Constructor_Test();

            LogMessageTest logMessageTest = new();

            logMessageTest.Setup();

            for (int i = 0; i < 1; i++)
            {           

                dbLogger.SaveLog(logMessageTest.logMessage);
               // dbLogger.SaveLog(logMessageTest.logMessage);

            }

        }



        [Test]
        public void DBLogger_Delete_OLD_Logs()
        {
            DBLogger_Constructor_Test();

             dbLogger.DeleteOldLogs(100);

        }


        [Test]
        public void PrintLogCount()
        {
            DBLogger_Constructor_Test();

            TestContext.WriteLine(dbLogger.GetLogCount);

        }




    }
}