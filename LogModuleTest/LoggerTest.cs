using LogModule;
using NUnit.Framework;
using System;

namespace LogModuleTest
{
    public class LoggerTest
    {

        [Test]
        public void LoggerExecuationTest()
        {


            using (Logger logger = new())
            {




                LogMessageTest logMessage = new(); // init a message sample
                FileLoggerTest fileLogger = new(); // init the file logger

                fileLogger.FileLogger_Constructor_Test(); // create the filelogger
                logMessage.Setup(); // create the message


                logger.RegisterLoggingChannel(fileLogger.fileLogger); // register filelogger in loggerchannels


                logger.LogInfo("This is an INFO message from the Test Project !",
                    Source: System.Reflection.Assembly.GetExecutingAssembly().GetName().Name
                                  + "." + this.GetType().Name + "." + System.Reflection.MethodBase.GetCurrentMethod().Name);

                logger.LogWarning("This is a WARNING message from the Test Project !",
                                   Source: System.Reflection.Assembly.GetExecutingAssembly().GetName().Name
                                  + "." + this.GetType().Name + "." + System.Reflection.MethodBase.GetCurrentMethod().Name);

                logger.LogError("This is an ERROR message from the Test Project !",
                                 Source: System.Reflection.Assembly.GetExecutingAssembly().GetName().Name
                                    + "." + this.GetType().Name + "." + System.Reflection.MethodBase.GetCurrentMethod().Name);



                logger.LogException(new InsufficientExecutionStackException());
                logger.LogException(new AccessViolationException());
                logger.LogException(new InsufficientMemoryException());

               


            }

            
            

        }






    }
}