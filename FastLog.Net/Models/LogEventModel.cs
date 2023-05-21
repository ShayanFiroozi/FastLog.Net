﻿
using FastLog.Enums;
using System;

namespace FastLog.Models
{
    public class LogEventModel
    {

        #region Constructors

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

        internal Exception Exception { get; private set; } = null;

        public LogEventTypes LogEventType { get; private set; }

        public string EventMessage { get; private set; }

        public string Details { get; private set; }


        #endregion



    }



}
