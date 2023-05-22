using FastLog.Agents;
using FastLog.Core;
using FastLog.Interfaces;
using FastLog.Models;
using LiteDB;
using System;
using System.IO;
using System.Threading;
using System.Threading.Tasks;

namespace LogModule.Agents
{
    public sealed class DBLogger : AgentBase<DBLogger>, IAgent
    {

        private string DBFile { get; set; }
        private short MaxDaysToKeepLogs { get; set; }


        public static DBLogger Create(AgentsManager manager) => new DBLogger(manager);

        private DBLogger(AgentsManager manager) 
        {
            _manager = manager;
        }


        public DBLogger WithDbFile(string fileName)
        {
            if (string.IsNullOrWhiteSpace(fileName))
            {
                throw new ArgumentException($"'{nameof(fileName)}' cannot be null or whitespace.", nameof(fileName));
            }

            DBFile = fileName;

            try
            {

                if (!Directory.Exists(Path.GetDirectoryName(DBFile)))
                {
                    _ = Directory.CreateDirectory(Path.GetDirectoryName(DBFile));
                }

                CleanUpTempDBFile();

            }

            catch (Exception ex)
            {
                InternalLogger?.LogInternalException(ex);
            }




            return this;
        }

        public DBLogger WithMaxDaysToKeepLogs(short maxDaysToKeepLogs)
        {

            if (maxDaysToKeepLogs <= 0)
            {
                throw new ArgumentException($"'{nameof(maxDaysToKeepLogs)}' must be greater then zero.", nameof(maxDaysToKeepLogs));
            }

            MaxDaysToKeepLogs = maxDaysToKeepLogs;

            return this;
        }


        private int GetLogCountInDB()
        {
            try
            {

                if (string.IsNullOrWhiteSpace(DBFile))
                {
                    throw new Exception("Db file is not defined.");
                }

                // Open or create the database
                using (LiteDatabase db = new LiteDatabase(DBFile))
                {
                    // Open or create the table
                    ILiteCollection<LogEventModel> dbTable = db.GetCollection<LogEventModel>();

                    return dbTable.Count();
                }

            }
            catch (Exception ex)
            {
                InternalLogger?.LogInternalException(ex);
                return 0;
            }
        }

        private void CleanUpTempDBFile()
        {
            // Delete the LiteDB temporary file is existed ( in case of power loss or app crash )

            try
            {

                string _targetLiteDBTempFile =
                    $"{Path.Combine(Path.GetDirectoryName(DBFile), Path.GetFileNameWithoutExtension(DBFile))}-log{Path.GetExtension(DBFile)}";

                if (File.Exists(_targetLiteDBTempFile))
                {
                    File.Delete(_targetLiteDBTempFile);
                }
            }
            catch { }

        }




        public void SaveLog(LogEventModel logMessage)
        {
            if (logMessage is null)
            {
                throw new ArgumentNullException(nameof(logMessage));
            }

            try
            {

                // Open or create the database
                using (LiteDatabase db = new LiteDatabase(DBFile))
                {

                    // Open or create the table
                    ILiteCollection<LogEventModel> dbTable = db.GetCollection<LogEventModel>();

                    dbTable.Insert(logMessage);
                }


            }
            catch (Exception ex)
            {
                InternalLogger?.LogInternalException(ex);
            }
        }



        public void SaveLog(LogEventModel logMessage, LiteDatabase logDB)
        {
            if (logMessage is null)
            {
                throw new ArgumentNullException(nameof(logMessage));
            }

            if (logDB is null)
            {
                throw new ArgumentNullException(nameof(logDB));
            }

            try
            {
                // Open or create the table
                ILiteCollection<LogEventModel> dbTable = logDB.GetCollection<LogEventModel>();

                dbTable.Insert(logMessage);


            }
            catch (Exception ex)
            {
                InternalLogger?.LogInternalException(ex);
            }


        }


        public void DeleteOldLogs(short OlderThanXDays)
        {
            int howManyDeleted = 0;

            try
            {

                if (!File.Exists(DBFile))
                {
                    return;
                }


                if (OlderThanXDays <= 0) return;


                DateTime targetDatetTime_For_OLD_Logs = DateTime.Now.AddDays(-OlderThanXDays);


                // Open or create the database
                using (LiteDatabase db = new LiteDatabase(DBFile))
                {

                    // Open or create the table
                    ILiteCollection<LogEventModel> dbTable = db.GetCollection<LogEventModel>();


                    if (dbTable.FindOne(log => log.DateTime < targetDatetTime_For_OLD_Logs) == null)
                    {
                        return; // No record to delete
                    }



                    howManyDeleted = dbTable.DeleteMany(log => log.DateTime < targetDatetTime_For_OLD_Logs);



                    _ = db.Rebuild(); // Shrink the database file ( remove unused spaces )

                }

                if (howManyDeleted != 0)
                {
                    InternalLogger?.LogInternalSystemEvent(new LogEventModel(FastLog.Enums.LogEventTypes.SYSTEM,
                                  $"The {howManyDeleted:N0} old record(s) have been deleted.",
                                  $"Reaches the maximum day(s) to keep logs , ({howManyDeleted:N0} day(s))"));
                }

            }
            catch (Exception ex)
            {
                InternalLogger?.LogInternalException(ex);
            }
        }

        public Task ExecuteAgent(LogEventModel logMessage, CancellationToken cancellationToken)
        {
            DeleteOldLogs(MaxDaysToKeepLogs);


            return Task.CompletedTask;
        }






    }
}
