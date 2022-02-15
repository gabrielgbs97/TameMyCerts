﻿// Copyright 2021 Uwe Gradenegger

// Licensed under the Apache License, Version 2.0 (the "License");
// you may not use this file except in compliance with the License.
// You may obtain a copy of the License at

// http://www.apache.org/licenses/LICENSE-2.0

// Unless required by applicable law or agreed to in writing, software
// distributed under the License is distributed on an "AS IS" BASIS,
// WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
// See the License for the specific language governing permissions and
// limitations under the License.

using System.Diagnostics;
using System.Security;

namespace TameMyCerts
{
    public class Logger
    {
        private readonly EventLog _eventLog;
        private readonly int _logLevel;

        public Logger(string eventSource, int logLevel = CertSrv.CERTLOG_WARNING)
        {
            _logLevel = logLevel;
            _eventLog = new EventLog("Application")
            {
                Source = CreateEventSource(eventSource)
            };
        }

        public void Log(Event logEvent, params object[] args)
        {
            if (_logLevel >= logEvent.LogLevel)
                _eventLog.WriteEntry(
                    string.Format(logEvent.MessageText, args),
                    logEvent.Type,
                    logEvent.ID);
        }

        private static string CreateEventSource(string currentAppName)
        {
            var eventSource = currentAppName;

            try
            {
                var sourceExists = EventLog.SourceExists(eventSource);
                if (!sourceExists)
                    EventLog.CreateEventSource(eventSource, "Application");
            }
            catch (SecurityException)
            {
                eventSource = "Application";
            }

            return eventSource;
        }
    }

    public static class Events
    {
        public static Event PDEF_SUCCESS_INIT = new Event
        {
            ID = 1,
            LogLevel = CertSrv.CERTLOG_VERBOSE,
            MessageText = LocalizedStrings.Events_PDEF_SUCCESS_INIT
        };

        public static Event PDEF_FAIL_INIT = new Event
        {
            ID = 2,
            LogLevel = CertSrv.CERTLOG_ERROR,
            Type = EventLogEntryType.Error,
            MessageText = LocalizedStrings.Events_PDEF_FAIL_INIT
        };

        public static Event PDEF_FAIL_VERIFY = new Event
        {
            ID = 3,
            LogLevel = CertSrv.CERTLOG_ERROR,
            Type = EventLogEntryType.Error,
            MessageText = LocalizedStrings.Events_PDEF_FAIL_VERIFY
        };

        public static Event PDEF_FAIL_SHUTDOWN = new Event
        {
            ID = 4,
            LogLevel = CertSrv.CERTLOG_ERROR,
            Type = EventLogEntryType.Error,
            MessageText = LocalizedStrings.Events_PDEF_FAIL_SHUTDOWN
        };

        public static Event REQUEST_DENIED_AUDIT = new Event
        {
            ID = 5,
            LogLevel = CertSrv.CERTLOG_MINIMAL,
            Type = EventLogEntryType.Warning,
            MessageText = LocalizedStrings.Events_REQUEST_DENIED_AUDIT
        };

        public static Event REQUEST_DENIED = new Event
        {
            ID = 6,
            Type = EventLogEntryType.Warning,
            MessageText = LocalizedStrings.Events_REQUEST_DENIED
        };

        public static Event POLICY_NOT_FOUND = new Event
        {
            ID = 7,
            Type = EventLogEntryType.Warning,
            MessageText = LocalizedStrings.Events_POLICY_NOT_FOUND
        };

        public static Event POLICY_NOT_APPLICABLE = new Event
        {
            ID = 8,
            LogLevel = CertSrv.CERTLOG_EXHAUSTIVE,
            Type = EventLogEntryType.Warning,
            MessageText = LocalizedStrings.Events_POLICY_NOT_APPLICABLE
        };

        public static Event MODULE_NOT_SUPPORTED = new Event
        {
            ID = 9,
            LogLevel = CertSrv.CERTLOG_ERROR,
            Type = EventLogEntryType.Error,
            MessageText = LocalizedStrings.Events_MODULE_NOT_SUPPORTED
        };

        public static Event REQUEST_DENIED_NO_TEMPLATE_INFO = new Event
        {
            ID = 10,
            LogLevel = CertSrv.CERTLOG_ERROR,
            Type = EventLogEntryType.Error,
            MessageText = LocalizedStrings.Events_REQUEST_DENIED_NO_TEMPLATE_INFO
        };

        public static Event REQUEST_DENIED_NO_POLICY = new Event
        {
            ID = 10,
            LogLevel = CertSrv.CERTLOG_ERROR,
            Type = EventLogEntryType.Error,
            MessageText = LocalizedStrings.Events_REQUEST_DENIED_NO_POLICY
        };

        public static Event PDEF_REQUEST_DENIED = new Event
        {
            ID = 11,
            LogLevel = CertSrv.CERTLOG_VERBOSE,
            MessageText = LocalizedStrings.Events_PDEF_REQUEST_DENIED
        };
    }

    public class Event
    {
        public int ID { get; set; }
        public int LogLevel { get; set; } = CertSrv.CERTLOG_WARNING;

        public EventLogEntryType Type { get; set; } = EventLogEntryType.Information;
        public string MessageText { get; set; }
    }
}