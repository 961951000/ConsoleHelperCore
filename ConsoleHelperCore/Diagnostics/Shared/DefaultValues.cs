using System;
using System.Collections.Generic;
using System.Text;
using ConsoleHelperCore.Diagnostics.Entities;

namespace ConsoleHelperCore.Diagnostics.Shared
{
    public class DefaultValues
    {
        public static readonly string StateNotSetToLogEntry = $"Warning : State not provided in correct Format type. Please provide state of type :{typeof(LogEntry)}";
        public static readonly string EventIdKey = "EventId";
        public static readonly string EventNameKey = "EventName";
        public static readonly string TypeNameKey = "Caller Type";
        public static readonly string CorrelationId = "CorrelationId";
        public static readonly string CallerMethodName = "CalledFromMethodName";
    }
}
