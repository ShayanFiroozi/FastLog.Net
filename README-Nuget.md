![FastLog.Net Logo](https://github.com/ShayanFiroozi/FastLog.Net/blob/master/FastLog.Net/Icon/FastLogNet.ico "FastLog.Net , High Performance Logger for .Net").

# **FastLog.Net**
**FastLog.Net** is the fastest , most efficient and high performance logger for **.Net** 
 
## **How To Use ❔**
 ### **Step 1 :**  
 - Build the **Internal Logger** agent with fluent builder pattern :  
 
 ```csharp
 InternalLogger internalLogger = InternalLogger.Create()
                                               .UseJsonFormat()
                                               .SaveInternalEventsToFile(@"Logs\InternalEventsLog.log")
                                               .DeleteTheLogFileWhenExceededTheMaximumSizeOf(20)
                                               .PrintOnConsole();
 ```   
   ***`Note`**: Internal logger agent is responsible for logging the events occured in the FastLog.Net internally (including exceptions).*  
 
  ### **Step 2 :**  
 - Build the **Logger Configuration** with fluent builder pattern :  
 
 ```csharp
 ConfigManager loggerConfig = ConfigManager.Create()
                                           .WithLoggerName("FastLog.Net® Logger")
                                           .WithMaxEventsToKeepInMemory(1_000);
 ```   
       ***`Note`**: There is "**RunAgentsInParallelMode**" feature you can use to run agent(s) in parallel , but in most cases it's **NOT** recommended because may have considerable negative impact on performance.*  
 
   ### **Step 3 :**  
 - Build the **Logger** with fluent builder pattern :  
 
 ```csharp
 Logger fastLogger = Logger.Create()
                           .WithInternalLogger(internalLogger)
                           .WithConfiguration(loggerConfig)
                           .WithAgents()
 
 
 // Add a "Console Agent" with Json format.
                              .AddConsoleAgent()
                               .UseJsonFormat()
                              .BuildAgent()
 
  
 
 // Add a "TextFile Agent" with Json format and will be re-created when reached to 10 megabytes.
                              .AddTextFileAgent()
                               .UseJsonFormat()
                               .SaveLogToFile("Logs\\TestLog.json")
                               .DeleteTheLogFileWhenExceededTheMaximumSizeOf(10)
                              .BuildAgent() 
 
 // And Finally build the logger.
                            .BuildLogger();
 
 
 // Start the FastLog.Net engine in the background.
      fastLogger.StartLogger();

 ``` 

 ### Final Step :  
  - **FastLog.Net** is ready , just call a logging method from anywhere of your code :  
 
 ```csharp
_= fastLogger.LogInfo("This is test logging INFO event);
 
await fastLogger.LogException(new InvalidOperationException());
 
await fastLogger.LogException(new Exception("This is a test exception i want to throw !!"));
 
await fastLogger.LogSystem("The system is gonna be restarted !");
 ```   

## GitHub Repository
Please visit FastLog.Net Github repository for source code and more info : [**FastLog.Net On GitHub**](https://github.com/ShayanFiroozi/FastLog.Net)
