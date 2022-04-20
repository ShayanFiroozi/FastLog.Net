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
                                        LogMessage.LogTypeEnum.ERROR, "This is a test message",
                                        "This is an error from the LogMessageTest module.",
                                        "LogModuleTest.LogMessageTest.Setup()");


            //logMessage = new LogMessage(
            //                           LogMessage.LogTypeEnum.ERROR, "This is a test message",
            //                           null,
            //                           "Setup() Method.");

        }



        [Test]
        public void Display_GetLogMessage_Data()
        {
            TestContext.WriteLine(logMessage.GetLogMessage());
        }




        [Test]
        public void Secret_Key()
        {
            TestContext.WriteLine(new NetMQServer.Core.Win32PipeMessage().SetMessageHash);
        }




    }
}