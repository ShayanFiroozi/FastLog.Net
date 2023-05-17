﻿
using FastLog.Enums;
using FastLog.Net.Helpers.ExtendedMethods;
using System;
using System.Text;

namespace TrendSoft.FastLog.Models
{
    public class LogEventModel
    {

        #region Constructors

        public LogEventModel(LogEventTypes LogEventType,
                             string EventText,
                             string Details = "",
                             bool LogMachineName = false,
                             string ApplicationName = "",
                             int EventId = 0)
        {
            DateTime = DateTime.Now;
            this.LogEventType = LogEventType;
            this.EventText = EventText;
            this.Details = Details;
            this.LogMachineName = LogMachineName;
            this.ApplicationName = ApplicationName;
            this.EventId = EventId;
        }


        public LogEventModel(Exception exception, bool LogMachineName = false, string ApplicationName = "", int EventId = 0)
            : this(LogEventTypes.EXCEPTION,

                   $"\nId : {exception?.HResult}\n" +
                   $"Message : {exception?.Message ?? "N/A"}\n",

                   $"InnerException : {exception?.InnerException?.Message ?? "N/A"}\n" +
                   $"StackTrace : {exception?.StackTrace ?? "N/A"}\n" +
                   $"Source : {exception?.Source ?? "N/A"}\n" +
                   $"Target Site : {(exception?.TargetSite != null ? exception?.TargetSite?.Name : "N/A")}",

                   LogMachineName,
                   ApplicationName,
                   EventId)
        { }



        public LogEventModel()
        {
            // This default constructor is neccessary for LiteDB.
        }

        #endregion


        #region Properties


        public int EventId { get; private set; } = 0;

        public DateTime DateTime { get; private set; }

        public LogEventTypes LogEventType { get; private set; }

        public string EventText { get; private set; }

        public string Details { get; private set; }

        public bool LogMachineName { get; private set; }

        public string ApplicationName { get; private set; }


        #endregion



    }


   
}
