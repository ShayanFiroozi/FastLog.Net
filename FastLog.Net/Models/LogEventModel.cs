/*---------------------------------------------------------------------------------------------

                ► FastLog.Net , High Performance Logger For .Net ◄



 → Copyright (c) 2020-2023 Shayan Firoozi , Bandar Abbas , Iran , Under MIT License.

 → Contact : <shayan.firoozi@gmail.com>

 → GitHub repository : https://github.com/ShayanFiroozi/FastLog.Net

---------------------------------------------------------------------------------------------*/

using FastLog.Enums;
using FastLog.Helpers.ExtendedMethods;
using System;

namespace FastLog.Models
{

    /// <summary>
    /// Event Model class
    /// </summary>
    public sealed class LogEventModel
    {

        #region Constructors

        /// <summary>
        /// Create user-define event with custom Event Type , Event Text and Event Details
        /// </summary>
        /// <param name="LogEventType">Event Type</param>
        /// <param name="EventText">Event Text</param>
        /// <param name="Details">Event Details</param>
        /// <param name="EventId">Event Id</param>
        public LogEventModel(LogEventTypes LogEventType,
                             string EventText,
                             string Details = "",
                             int EventId = 0)
        {
            DateTime = DateTime.Now;
            this.LogEventType = LogEventType;
            this.EventMessage = EventText;
            this.Details = Details;
            this.EventId = EventId;
        }


        /// <summary>
        /// Create Exception event.
        /// </summary>
        /// <param name="exception">Exception to save.</param>
        /// <param name="EventId">User defined Event Id.</param>
        public LogEventModel(Exception exception, int EventId = 0)
            : this(LogEventTypes.EXCEPTION,

                   $"\nId : {exception?.HResult}\n" +
                   $"Message : {exception?.Message ?? "N/A"}\n",

                   $"InnerException : {exception?.InnerException?.Message ?? "N/A"}\n" +
                   $"StackTrace : {exception?.StackTrace ?? "N/A"}\n" +
                   $"Source : {exception?.Source ?? "N/A"}\n" +
                   $"Target Site : {(exception?.TargetSite != null ? exception?.TargetSite?.Name : "N/A")}",
                   EventId)
        {
            this.Exception = exception;
        }



        public LogEventModel()
        {
            // This default constructor is neccessary for LiteDB.
        }

        #endregion


        #region Properties


        public int EventId { get; private set; } = 0;

        public DateTime DateTime { get; private set; }

        /// <summary>
        /// Store exception object here to access them "Logger.InMemoryEvents" class.
        /// </summary>
        internal Exception Exception { get; private set; } = null;

        public LogEventTypes LogEventType { get; private set; }

        public string EventMessage { get; private set; }

        public string Details { get; private set; }


        #endregion

        public override string ToString()
        {
            return this.ToPlainText();
        }

    }



}
