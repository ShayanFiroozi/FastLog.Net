using LiteDB;
using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;

namespace LogModule.Agents
{
    public sealed class DBLogger : IDBLogger
    {

        #region ReadOnly Variables
        private readonly int LOG_FILE_MAX_SIZE_IN_MB = 0; // 0 for unlimited file size

        private readonly string _LogFile; 
        #endregion



        #region Properties


        public string LogFile => _LogFile;


        public int LogFileSizeMB
        {
            get
            {
                try
                {
                    return Convert.ToInt32((new FileInfo(LogFile).Length / 1024) / 1024);
                }
                catch (Exception ex)
                {
                    InnerException.InnerException.LogInnerException(ex);
                    return 0;
                }
            }
        }

        public int GetLogCount
        {
            get
            {
                try
                {

                    // Open or create the database
                    using (LiteDatabase db = new(LogFile))
                    {


                        // Open or create the table
                        ILiteCollection<LogMessage> dbTable = db.GetCollection<LogMessage>();

                        return dbTable.Count();

                    }

                }
                catch (Exception ex)
                {
                    InnerException.InnerException.LogInnerException(ex);
                    return 0;
                }
            }
        }




        #endregion



        #region Constructors


        public DBLogger(string LogFile,
                          int LOG_FILE_MAX_SIZE_IN_MB = 100)
        {

            try
            {

                if (string.IsNullOrWhiteSpace(LogFile))
                {
                    throw new ArgumentNullException("Invalid logging path or file name");
                }


                this._LogFile = LogFile;
                this.LOG_FILE_MAX_SIZE_IN_MB = LOG_FILE_MAX_SIZE_IN_MB;


                // Create the log file directory

                Directory.CreateDirectory(new FileInfo(this.LogFile).Directory.FullName);



                // Delete the log file if it's bigger than LOG_FILE_MAX_SIZE_IN_MB

                DeleteLogFile();


            }
            catch (Exception ex)
            {
                InnerException.InnerException.LogInnerException(ex);
            }
        }


        #endregion



        #region Methods




        /// <summary>
        /// Save the log (LiteDatabase will be initialized internally)
        /// </summary>
        /// <param name="logMessage">LogMessage object to save</param>
        public void SaveLog(LogMessage logMessage)
        {

            try
            {

                if (logMessage == null)
                {
                    throw new ArgumentNullException("logMessage parameter can not be null.");
                }

                // Open or create the database
                using (LiteDatabase db = new(LogFile))
                {

                    // Open or create the table
                    ILiteCollection<LogMessage> dbTable = db.GetCollection<LogMessage>();

                    dbTable.Insert(logMessage);
                }


            }
            catch (Exception ex)
            {
                InnerException.InnerException.LogInnerException(ex);
            }
        }



        /// <summary>
        /// Save the log with initialized LiteDatabase object ( dependency injection )
        /// </summary>
        /// <param name="logMessage">LogMessage object to save</param>
        /// <param name="logDB">Initialized LiteDatabase  object ( dependency injection )</param>

        public void SaveLog(LogMessage logMessage, LiteDatabase logDB)
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
                ILiteCollection<LogMessage> dbTable = logDB.GetCollection<LogMessage>();

                dbTable.Insert(logMessage);


            }
            catch (Exception ex)
            {
                InnerException.InnerException.LogInnerException(ex);
            }


        }





        public void DeleteLogFile()
        {
            try
            {

                if (!File.Exists(LogFile))
                {
                    return;
                }


                if (LogFileSizeMB
                     <= LOG_FILE_MAX_SIZE_IN_MB || LOG_FILE_MAX_SIZE_IN_MB == 0)
                {
                    return;
                }
            }
            catch (Exception ex)
            {
                InnerException.InnerException.LogInnerException(ex);
            }




            try
            {
                File.Delete(LogFile);
            }
            catch (Exception ex)
            {
                InnerException.InnerException.LogInnerException(ex);
            }
        }

        public void DeleteOldLogs(short OlderThanDays)
        {
            try
            {

                if (OlderThanDays <= 0) return;


                // Open or create the database
                using (LiteDatabase db = new(LogFile))
                {


                    // Open or create the table
                    ILiteCollection<LogMessage> dbTable = db.GetCollection<LogMessage>();




                    _ = dbTable.DeleteMany(log => log.DateTime < DateTime.Now.AddDays(-OlderThanDays));

                    _ = db.Rebuild(); // Shrink the database file ( remove unused spaces )


                }

            }
            catch (Exception ex)
            {
                InnerException.InnerException.LogInnerException(ex);
            }
        }


        #endregion



    }
}
