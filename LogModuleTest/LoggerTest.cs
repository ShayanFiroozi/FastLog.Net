using LogModule;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LogModuleTest
{
    public class LoggerTest
    {

        [Test]
        public async Task LoggerExecuationTestAsync()
        {

            List<Task> taskList = new();

            using (Logger logger = new())
            {


                LogMessageTest logMessage = new(); // init a message sample
                FileLoggerTest fileLogger = new(); // init the file logger
                DBLoggerTest dbLogger = new(); // init the db logger

                fileLogger.FileLogger_Constructor_Test(); // create the filelogger
                dbLogger.DBLogger_Constructor_Test(); // create the dblogger

                logMessage.Setup(); // create the message


                logger.RegisterLoggingAgent(fileLogger.fileLogger); // register filelogger in loggerchannels
                logger.RegisterLoggingAgent(dbLogger.dbLogger); // register dblogger in loggerchannels


                Parallel.For(0, 500, (i) =>

               {

                   taskList.Add(Task.Run(() =>
                    {

                        logger.LogInfo("This is an INFO message from the Test Project !",
                         Source: System.Reflection.Assembly.GetExecutingAssembly().GetName().Name
                                       + "." + GetType().Name + "." + System.Reflection.MethodBase.GetCurrentMethod().Name);

                        logger.LogWarning("This is a WARNING message from the Test Project !",
                                            Source: System.Reflection.Assembly.GetExecutingAssembly().GetName().Name
                                           + "." + GetType().Name + "." + System.Reflection.MethodBase.GetCurrentMethod().Name);

                        logger.LogError("This is an ERROR message from the Test Project !",
                                          Source: System.Reflection.Assembly.GetExecutingAssembly().GetName().Name
                                             + "." + GetType().Name + "." + System.Reflection.MethodBase.GetCurrentMethod().Name);


                        logger.LogDebug("This is a DEBUG message from the Test Project !",
                                          Source: System.Reflection.Assembly.GetExecutingAssembly().GetName().Name
                                             + "." + GetType().Name + "." + System.Reflection.MethodBase.GetCurrentMethod().Name);



                        logger.LogException(new InsufficientExecutionStackException());
                        logger.LogException(new AccessViolationException());
                        logger.LogException(new InsufficientMemoryException());


                    }));


               });

                await Task.WhenAll(taskList.Where(t=> t is not null));


            }


        }
    }

}