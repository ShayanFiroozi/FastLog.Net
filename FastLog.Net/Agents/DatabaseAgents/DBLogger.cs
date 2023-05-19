//using LiteDB;
//using System;
//using System.Collections.Generic;
//using System.IO;
//using System.Threading.Tasks;

//namespace LogModule.Agents
//{
//    public  class DBLogger : IDBLogger
//    {

//        #region ReadOnly Variables
//        private readonly int LOG_FILE_MAX_SIZE_IN_MB = 0; // 0 for unlimited file size

//        private readonly string _LogFile;
//        #endregion



//        #region Properties


//        public string LogFile => _LogFile;


//        public int LogFileSizeMB
//        {
//            get
//            {
//                try
//                {
//                    return Convert.ToInt32((new FileInfo(LogFile).Length / 1024) / 1024);
//                }
//                catch (Exception ex)
//                {
//                    InnerException.InnerException.LogInnerException(ex);
//                    return 0;
//                }
//            }
//        }

//        public int GetLogCount
//        {
//            get
//            {
//                try
//                {

//                    // Open or create the database
//                    using (LiteDatabase db = new(LogFile))
//                    {


//                        // Open or create the table
//                        ILiteCollection<LogMessage> dbTable = db.GetCollection<LogMessage>();

//                        return dbTable.Count();

//                    }

//                }
//                catch (Exception ex)
//                {
//                    InnerException.InnerException.LogInnerException(ex);
//                    return 0;
//                }
//            }
//        }




//        #endregion



//        #region Constructors


//        public DBLogger(string LogFile,
//                          int LOG_FILE_MAX_SIZE_IN_MB = 200,
//                          short LOGS_ARE_OLDER_THAN_X_DAYS = 365)
//        {

//            try
//            {

//                if (string.IsNullOrWhiteSpace(LogFile))
//                {
//                    throw new ArgumentNullException("Invalid logging path or file name");
//                }


//                this._LogFile = LogFile;
//                this.LOG_FILE_MAX_SIZE_IN_MB = LOG_FILE_MAX_SIZE_IN_MB;


//                // Create the log file directory

//                Directory.CreateDirectory(new FileInfo(this.LogFile).Directory.FullName);





//                // Delete the LiteDB temporary file is existed ( in case of power loss or app crash )

//                try
//                {

//                    string _targetLiteDBTempFile = 
//                        $"{Path.Combine(Path.GetDirectoryName(LogFile),Path.GetFileNameWithoutExtension(LogFile))}-log{Path.GetExtension(LogFile)}";

//                    if (File.Exists(_targetLiteDBTempFile))
//                    {
//                        File.Delete(_targetLiteDBTempFile);
//                    }
//                }
//                catch { }




//                // Delete the log file if it's bigger than LOG_FILE_MAX_SIZE_IN_MB

//                DeleteLogFile();



//                // Delete the logs they are older than  MAX_OLD_DAYS_LOGS_IN_DATABSE day(s)

//                DeleteOldLogs(LOGS_ARE_OLDER_THAN_X_DAYS);




//            }
//            catch (Exception ex)
//            {
//                InnerException.InnerException.LogInnerException(ex);
//            }
//        }


//        #endregion



//        #region Methods




//        /// <summary>
//        /// Save the log (LiteDatabase will be initialized internally)
//        /// </summary>
//        /// <param name="logMessage">LogMessage object to save</param>
//        public void SaveLog(LogMessage logMessage)
//        {

//            try
//            {

//                if (logMessage == null)
//                {
//                    throw new ArgumentNullException("logMessage parameter can not be null.");
//                }

//                // Open or create the database
//                using (LiteDatabase db = new(LogFile))
//                {

//                    // Open or create the table
//                    ILiteCollection<LogMessage> dbTable = db.GetCollection<LogMessage>();

//                    dbTable.Insert(logMessage);
//                }


//            }
//            catch (Exception ex)
//            {
//                InnerException.InnerException.LogInnerException(ex);
//            }
//        }



//        /// <summary>
//        /// Save the log with initialized LiteDatabase object ( dependency injection )
//        /// </summary>
//        /// <param name="logMessage">LogMessage object to save</param>
//        /// <param name="logDB">Initialized LiteDatabase  object ( dependency injection )</param>

//        public void SaveLog(LogMessage logMessage, LiteDatabase logDB)
//        {
//            if (logMessage is null)
//            {
//                throw new ArgumentNullException(nameof(logMessage));
//            }

//            if (logDB is null)
//            {
//                throw new ArgumentNullException(nameof(logDB));
//            }


//            try
//            {
//                // Open or create the table
//                ILiteCollection<LogMessage> dbTable = logDB.GetCollection<LogMessage>();

//                dbTable.Insert(logMessage);


//            }
//            catch (Exception ex)
//            {
//                InnerException.InnerException.LogInnerException(ex);
//            }


//        }





//        public void DeleteLogFile()
//        {
//            try
//            {

//                if (!File.Exists(LogFile))
//                {
//                    return;
//                }


//                if (LogFileSizeMB
//                     <= LOG_FILE_MAX_SIZE_IN_MB || LOG_FILE_MAX_SIZE_IN_MB == 0)
//                {
//                    return;
//                }
//            }
//            catch (Exception ex)
//            {
//                InnerException.InnerException.LogInnerException(ex);
//            }




//            try
//            {
//                File.Delete(LogFile);

//                SaveLog(new LogMessage(LogMessage.LogTypeEnum.INFO,
//                                      "The Log file has been deleted.",
//                                      $"Reaches the maximum file size ({LOG_FILE_MAX_SIZE_IN_MB:N0} MB)"));
//            }
//            catch (Exception ex)
//            {
//                InnerException.InnerException.LogInnerException(ex);
//            }
//        }

//        public void DeleteOldLogs(short OlderThanxDays)
//        {
//            int _howManyDeleted = 0;



//            try
//            {

//                if (!File.Exists(LogFile))
//                {
//                    return;
//                }


//                if (OlderThanxDays <= 0) return;


//                DateTime targetDatetTime_For_OLD_Logs = DateTime.Now.AddDays(-OlderThanxDays);


//                // Open or create the database
//                using (LiteDatabase db = new(LogFile))
//                {

//                    // Open or create the table
//                    ILiteCollection<LogMessage> dbTable = db.GetCollection<LogMessage>();


//                    if (dbTable.FindOne(log => log.DateTime < targetDatetTime_For_OLD_Logs) == null)
//                    {
//                        return; // No record to delete
//                    }



//                    _howManyDeleted = dbTable.DeleteMany(log => log.DateTime < targetDatetTime_For_OLD_Logs);



//                    _ = db.Rebuild(); // Shrink the database file ( remove unused spaces )

//                }

//                if (_howManyDeleted != 0)
//                {
//                    SaveLog(new LogMessage(LogMessage.LogTypeEnum.INFO,
//                                  $"The {_howManyDeleted:N0} pld log records have been deleted.",
//                                  $"Reaches the maximum old logs days ({OlderThanxDays:N0} day(s))"));
//                }

//            }
//            catch (Exception ex)
//            {
//                InnerException.InnerException.LogInnerException(ex);
//            }
//        }


//        #endregion



//    }
//}
