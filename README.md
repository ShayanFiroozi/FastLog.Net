<p align="center">
 <img src="https://github.com/ShayanFiroozi/FastLog.Net/blob/master/FastLog.Net/Icon/FastLogNet.ico"
</p>

# FastLog.Net
**FastLog.Net** is the fastest , most efficient and high performance logger for **.Net** 
 
✔ Easy to use and develop , clean code , extensible agents and fully thread-safe.  
 
✔ FastLog.Net supports structured logging and enhanced Json format for [Exception](https://learn.microsoft.com/en-us/dotnet/api/system.exception?view=net-7.0) class.  
 
✔ FastLog.Net uses thread-safe queueing technique to enqueue the logging request(s) and release your thread  
     almost immediately after you call a logging method.

<br/>

## 💯Features
 **FastLog.Net features :**
 * **Supported Logging Events :**  
    * INFO  
    * NOTE  
    * TODO  
    * WARNING  
    * ALERT  
    * DEBUG  
    * ERROR  
    * EXCEPTION  (specially design for .Net [Exception](https://learn.microsoft.com/en-us/dotnet/api/system.exception?view=net-7.0) class to keep all details with nice json format)
    * SYSTEM  
    * SECURITY  
 <br/>  
 
 * **Agents :** 
   * **Text File Agent** which supports both plain text and json format with ability to handle log file max size.  
 
   * **Colorized Console Agent** which also supports text and json output.  
 
   * **Beep Agent** (works only on Windows® OS) which is resposible to make [beep](https://learn.microsoft.com/en-us/dotnet/api/system.console.beep?view=net-7.0) sound from BIOS or speaker with fully customizable event type.  
 
   * **Debug/Trace Agents** help you to log your events on [debug](https://learn.microsoft.com/en-us/dotnet/api/system.diagnostics.debug?view=net-7.0) or [trace](https://learn.microsoft.com/en-us/dotnet/api/system.diagnostics.trace?view=net-7.0) system.  
 
   * **Run Process Agent** will run a specific process in "Noraml" and "As administrator" privileges ,  based on a specific or all events.  
 
   * **Method Execution Agent** will call a method in your code ,  based on a specific or all events.  
 
   * **Heavy Operation Simulator Agent** simulates an heavy IO bound operation (like email sending or http operation) , this agent is used for testing the FastLog.Net or create a delay intentionally between the agents.  
 
      * **Note:** All agents support Event Type filtering ,  so you can easily specify that your agent should be executed on which event type(s).  
                  *For example: You can add a "Beep Agent" with "Exception" log event , so the beep agent will only execute on "Exceptions" not other logging events.*  
 
 <br/>  
 
 * **Internal Events Logger:**
   * **FastLog.Net** has a powerful and structured internal logging system to log and monitor internal exceptions or event which occured in the library itself !  
  <br/>  
 
 * **Thread Safety:**
   * **FastLog.Net** has an internal queue list which is designed fully thread-safe , it's really not matter how many thread(s) call the logging methods simultaneously , it gives your request(s) and put in the queue and immediately release your thread ! Then process your logging agent(s) in the background.
 
<br/>

## 🤝Contributions
As this is a new repository , there's no contributor yet! , But **FastLog.Net** welcomes and appreciates any contribution , pull request or bug report.

If you'd like to contribute, please read the [**How It Works**](https://github.com/ShayanFiroozi/FastLog.Net#-how-it-works) section and then take a look at [ToDo List](ToDo.md) to get involved !
 
Note : The srouce code is fully commented.

<br/>
 
## ❔ How To Use

<br/>
 
## ❓ How It Works

<br/>
 
## ❌ Limitations

- However the **FastLog.Net** is thread-safe BUT it is **NOT** recommended to build two agents with same logging file , so this limitation applied to the FastLog.Net intentionally to prevent two or more agents write and manage the same logging file.

- The **FastLog.Net** queue has been limited to handle up to the **1,000,000** logging events at the same time.This limitation has been set to prevent uncontrolled memory usage.

<br/>
 
## ⁉ Known Issues
 **Not Reported Yet!** 😎

<br/>
 
 ## © License
**FastLog.Net** is an open source software, licensed under the terms of MIT license.
See [LICENSE](LICENSE.md) for more details.

<br/>
 
## 🛠 How to build
Use **Visual Studio 2022** and open the solution 'FastLog.Net.sln'.

**FastLog.Net** solution is setup to support following .Net versions :

- .Net Core 7.0
- .Net Core 6.0
- .Net Framework 4.8


Note : Since the **FastLog.Net** solution is supporting multi target frameworks , to build the solution successfully you should install all .Net versions above , otherwise you can easily exclude not interested framework(s) by editing **TargetFrameworks** tag in the [FastLog.Net Project File](https://github.com/ShayanFiroozi/FastLog.Net/blob/master/FastLog.Net/FastLog.Net.csproj).

<br/>
 
## Donations
If you would like to financially support **FastLog.Net**, first of all, thank you! Please read [DONATIONS](DONATIONS.md) for my crypto wallets !

<br/>
 
## Version History
Please read [CHANGELOG](CHANGELOG.md) for more and track changing details.
