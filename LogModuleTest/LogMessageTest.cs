using LogModule;
using NUnit.Framework;

namespace LogModuleTest
{
    public class LogMessageTest
    {

        public LogMessage logMessage;

        [SetUp]
        public void Setup()
        {
            logMessage = new LogMessage(
                                        LogMessage.LogTypeEnum.ERROR,
                                        "LogMessageTest.LogMessage_Constructor_Test()",
                                        "Testing");

        }



        [Test]
        public void LogMessage_Constructor_Test()
        {
          
            Assert.That(
                        logMessage.LogType == LogMessage.LogTypeEnum.ERROR
                         &&
                        logMessage.Source == "LogMessageTest.LogMessage_Constructor_Test()"
                         &&
                        logMessage.LogText == "Testing"
                         &&
                        logMessage.ExtraInfo == string.Empty);


        }


        [Test]
        public void Display_GetLogMessage_Data()
        {
            TestContext.WriteLine(logMessage.GetLogMessage());
        }





    }
}