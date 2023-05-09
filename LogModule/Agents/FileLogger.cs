﻿
using System;
using System.IO;
using System.Threading;
using TrendSoft.LogModule.Interfaces;
using TrendSoft.LogModule.InternalException;
using TrendSoft.LogModule.Models;

namespace TrendSoft.LogModule.Agents
{
    public class FileLogger : IFileLogger
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
                    InternalExceptionLogger.LogInternalException(ex);
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


                _LogFile = LogFile;
                this.LOG_FILE_MAX_SIZE_IN_MB = LOG_FILE_MAX_SIZE_IN_MB;


                // Create the log file directory

                Directory.CreateDirectory(new FileInfo(this.LogFile).Directory.FullName);



                // Delete the log file if it's bigger than LOG_FILE_MAX_SIZE_IN_MB

                DeleteLogFile();


            }
            catch (Exception ex)
            {
                InternalExceptionLogger.LogInternalException(ex);
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
                InternalExceptionLogger.LogInternalException(ex);
            }




            try
            {
                File.Delete(LogFile);

                SaveLog(new LogMessageModel(LogMessageModel.LogTypeEnum.INFO,
                                       "The Log file has been deleted.",
                                       $"Reached the maximum file size ({LOG_FILE_MAX_SIZE_IN_MB:N0} MB)"));

            }
            catch (Exception ex)
            {
                InternalExceptionLogger.LogInternalException(ex);
            }
        }



        public void SaveLog(LogMessageModel logMessage)
        {

            try
            {

                if (logMessage == null)
                {
                    throw new ArgumentNullException("logMessage parameter can not be null.");
                }


                WriteToFileThreadSafe(LogFile, logMessage.GetLogMessage().ToString());


            }
            catch (Exception ex)
            {
                InternalExceptionLogger.LogInternalException(ex);
            }
        }


        #endregion


        private static readonly ReaderWriterLockSlim _readWriteLock = new();

        private void WriteToFileThreadSafe(string path, string text)
        {
            // Set Status to Locked
            _readWriteLock.EnterWriteLock();
            try
            {
                // Append text to the file
                using (StreamWriter sw = File.AppendText(path))
                {
                    sw.WriteLine(text);
                    sw.Close();
                }
            }
            finally
            {
                // Release lock
                _readWriteLock.ExitWriteLock();
            }
        }



    }
}
