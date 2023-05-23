# FastLog.Net
High performance logger module for .Net


# Known Issues : 
1- Accessing Logger "InMemoryEvents" property from multi thread causing an exception despute of using "lock" and "ReaderWriterLockSlim".