﻿using System;
using System.IO;
using System.Threading.Tasks;

namespace LogModule.Agents
{
    public sealed class FileLogger : IFileLogger
    {


        private readonly int LOG_FILE_MAX_SIZE_IN_MB = 0;

        private readonly string _LogFile;


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



        #endregion


        #region Constructors


        public FileLogger(string LogFile,
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


        public void DeleteLogFile()
        {
            try
            {

                if (!File.Exists(LogFile))
                {
                    return;
                }


                if (LogFileSizeMB
                     <= LOG_FILE_MAX_SIZE_IN_MB)
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


   
        public void SaveLog(LogMessage logMessage)
        {

            try
            {

                if (logMessage == null)
                {
                    throw new ArgumentNullException("logMessage parameter can not be null.");
                }

                using (StreamWriter streamWriter = new(LogFile, append: true))
                {
                    streamWriter.WriteLine(logMessage.GetLogMessage());

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