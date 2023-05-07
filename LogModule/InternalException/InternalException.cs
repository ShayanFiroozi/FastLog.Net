using System;
using System.IO;
using System.Threading.Channels;
using System.Threading.Tasks;

namespace LogModule.InnerException
{
    public static class InternalExceptionLogger
    {

        #region Channel Definitions
        private static Channel<Exception> InternalExceptionsChannel = Channel.CreateUnbounded<Exception>(new UnboundedChannelOptions());

        private static ChannelReader<Exception> InternalExceptionsChannelReader = InternalExceptionsChannel.Reader;
        private static ChannelWriter<Exception> InternalExceptionsChannelWriter = InternalExceptionsChannel.Writer;
        #endregion


        #region Properties
        private static string InternalExceptionsLogFile { get; set; } = string.Empty;
        private static short InternalExceptionsMaxLogFileMB { get; set; } = 0;
        private static bool reflectOnConsole { get; set; } = false;

        #endregion



        public static void SetLogFile(string internalExceptionsLogFile)
        {
            if (string.IsNullOrWhiteSpace(internalExceptionsLogFile))
            {
                throw new ArgumentException($"'{nameof(internalExceptionsLogFile)}' cannot be null or whitespace.", nameof(internalExceptionsLogFile));
            }


            InternalExceptionsLogFile = internalExceptionsLogFile;


            if (!Directory.Exists(Path.GetDirectoryName(internalExceptionsLogFile)))
            {
                Directory.CreateDirectory(internalExceptionsLogFile);
            }


        }

        public static void SetLogFileMaxSizeMB(short internalExceptionsMaxLogFileMB)
        {

            if (internalExceptionsMaxLogFileMB <= 0)
            {
                throw new ArgumentException($"'{nameof(internalExceptionsMaxLogFileMB)}' must be greater then zero.", nameof(internalExceptionsMaxLogFileMB));
            }

            InternalExceptionsMaxLogFileMB = internalExceptionsMaxLogFileMB;


        }

        public static void ReflectOnConsole() => reflectOnConsole = true;

        public static void UnReflectOnConsole() => reflectOnConsole = false;

        public static Task StartLogger()
        {
            if (string.IsNullOrWhiteSpace(InternalExceptionsLogFile))
            {
                throw new ArgumentException($"'{nameof(InternalExceptionsLogFile)}' cannot be null or whitespace.", nameof(InternalExceptionsLogFile));
            }


            if (InternalExceptionsMaxLogFileMB <= 0)
            {
                throw new ArgumentException($"'{nameof(InternalExceptionsMaxLogFileMB)}' must be greater then zero.", nameof(InternalExceptionsMaxLogFileMB));
            }

            return StartInternalExceptionLoggerEngine();
        }


        public static ValueTask LogInternalException(Exception exception)
        {
            return InternalExceptionsChannelWriter.WriteAsync(exception);
        }




        #region Private Functions


        private static Task WriteExceptionToFile(Exception exception)
        {
            if (string.IsNullOrWhiteSpace(InternalExceptionsLogFile))
            {
                throw new ArgumentException($"'{nameof(InternalExceptionsLogFile)}' cannot be null or whitespace.", nameof(InternalExceptionsLogFile));
            }

            if (InternalExceptionsMaxLogFileMB <= 0)
            {
                throw new ArgumentException($"'{nameof(InternalExceptionsMaxLogFileMB)}' must be greater then zero.", nameof(InternalExceptionsMaxLogFileMB));
            }


            if (exception == null) return Task.CompletedTask;


            try
            {


                return File.AppendAllTextAsync(InternalExceptionsLogFile, new LogModel(LogModel.LogTypeEnum.EXCEPTION,
                                               " Message : " + exception.Message ?? "-",
                                               " InnerMessage : " + (exception.InnerException?.Message ?? "-") +
                                               " , " +
                                               " StackTrace : " + (exception.StackTrace ?? "-"),
                                                (exception.Source ?? "-")).GetLogMessage().ToString() + Environment.NewLine);

            }
            catch
            {
                return Task.CompletedTask;
            }

        }


        private static Task StartInternalExceptionLoggerEngine()
        {
            return Task.Run(async () =>
             {

                 while (!InternalExceptionsChannelReader.Completion.IsCompleted)
                 {
                     Exception exceptionFromChannel = await InternalExceptionsChannelReader.ReadAsync();

                     if (exceptionFromChannel != null)
                     {
                         // Reflect on Console
                         if (reflectOnConsole)
                         {
                             Console.ForegroundColor = ConsoleColor.Red;
                             await Console.Out.WriteLineAsync($"{DateTime.Now}  \"Logger Internal Exception\" thrown : {exceptionFromChannel}");
                             Console.ForegroundColor = ConsoleColor.White;
                         }

                         // Delete the log file if it's getting big !
                         await DeleteInternalExceptionsLogFile();

                         // Save to the log file.
                         await WriteExceptionToFile(exceptionFromChannel);

                     }

                 }

             });
        }



        private static Task<short> GetLogFileSizeMB()
        {
            try
            {
                if (!string.IsNullOrWhiteSpace(InternalExceptionsLogFile)) return Task.FromResult<short>(0);
                if (!File.Exists(InternalExceptionsLogFile)) return Task.FromResult<short>(0);

                return Task.FromResult<short>((short)((new FileInfo(InternalExceptionsLogFile).Length / 1024) / 1024));
            }
            catch
            {
                return Task.FromResult<short>(0);
            }
        }

        private static async Task DeleteInternalExceptionsLogFile()
        {
            try
            {
                if (!string.IsNullOrWhiteSpace(InternalExceptionsLogFile)) return;
                if (!File.Exists(InternalExceptionsLogFile)) return;


                if (await GetLogFileSizeMB() <= InternalExceptionsMaxLogFileMB)
                {
                    return;
                }
                else
                {
                    File.Delete(InternalExceptionsLogFile);
                }
            }
            catch 
            {
               
            }

        }

        #endregion



    }

}

