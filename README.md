![alt tag](https://github.com/ShayanFiroozi/FastLog.Net/blob/master/FastLog.Net/Icon/FastLogNet.ico)

# FastLog.Net

**FastLog.Net** is the fastest , most efficient and high performance logger for .Net  
Easy to use and develop , clean code , extensible agents and fully thread-safe.  
FastLog.Net supports structured logging and enhanced Json format for Exceptions class.  
FastLog.Net uses thread-safe queueing technique to enqueue the logging request(s) and release your thread almost immediately after you log an event.

## Contributions

As this is a new repository , there's no contributor yet! , But **FastLog.Net** welcomes and appreciates any contribution , pull request or bug report.

If you'd like to contribute, please read the **How It Works** section and then take a look at [Todo.md](Todo.md) ! 😎


# How To Use


# How It Works


# ❌ Limitations

However **FastLog.Net** is thread-safe BUT it is **NOT** recommended to build two agents with same logging file , so this limitation applied to the FastLog.Net intentionally to prevent two or more agents write and manage same logging file.

# ⁉ Known Issues


License
---
**FastLog.Net** is an open source software, licensed under the terms of MIT license.
See [LICENSE.md](LICENSE.md) for more details.


How to build
---
Use **Visual Studio 2022** and open the solution 'FastLog.Net.sln'.

**FastLog.Net** solution is setup to support following .Net versions :

- .Net Core 7.0
- .Net Core 6.0
- .Net Framework 4.8


Note : Since the **FastLog.Net** solution is supporting multi target frameworks , to build the solution successfully you should install all .Net versions above , otherwise you can easily exclude not interested framework(s) by editing [FastLog.Net.csproj](FastLog.Net project file).

# Donations
If you would like to financially support **FastLog.Net**, first of all, thank you! Please read [DONATIONS.md](DONATIONS.md) for my crypto wallets !

# Version History
Please read [CHANGELOG.md](CHANGELOG.md) for more and track changing details.
