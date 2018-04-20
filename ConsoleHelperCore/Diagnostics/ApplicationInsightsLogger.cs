using System;
using System.Collections.Generic;
using System.Text;
using ConsoleHelperCore.Diagnostics.Entities;
using ConsoleHelperCore.Diagnostics.Interface;
using ConsoleHelperCore.Diagnostics.Shared;
using Microsoft.ApplicationInsights;
using Microsoft.ApplicationInsights.DataContracts;

namespace ConsoleHelperCore.Diagnostics
{
    public class ApplicationInsightsLogger<T> : ILogger<T>
    {
        private readonly TelemetryClient _telemetryClient;
        private readonly LogOptions _options;

        public ApplicationInsightsLogger(TelemetryClient client, LogOptions options)
        {
            _telemetryClient = client ?? throw new ArgumentNullException(nameof(client));
            _options = options ?? throw new ArgumentNullException(nameof(options));
        }

        public void Log<TState>(LogLevel logLevel, TState state, Exception exception,
            Func<TState, Exception, string> formatter)
        {
            if (!IsEnabled(logLevel)) return;
            var logEntry = state as LogEntry;
            if (exception == null)
            {
                LogTrace(logLevel, logEntry);
            }
            else
            {
                LogException(logLevel, state, exception, formatter);
            }
        }



        public bool IsEnabled(LogLevel logLevel)
        {
            return logLevel >= _options.LogLevel;
        }

        public IDisposable BeginScope<TState>(TState state)
        {
            return null;
        }

        #region Private Methods

        private void LogException<TState>(LogLevel logLevel, TState state, Exception exception, Func<TState, Exception, string> formatter)
        {
            var exceptionTelemetry = new ExceptionTelemetry(exception)
            {
                Message = formatter(state, exception),
                SeverityLevel = GetSeverityLevel(logLevel)
            };
            exceptionTelemetry.Context.Properties["Exception"] = exception.ToString();
            _telemetryClient.TrackException(exceptionTelemetry);
        }

        private void LogTrace(LogLevel logLevel, LogEntry logEntry)
        {
            if (logEntry == null)
                _telemetryClient.TrackTrace(DefaultValues.StateNotSetToLogEntry, GetSeverityLevel(logLevel),
                    PopulateLogAttributes());
            else
                _telemetryClient.TrackTrace(logEntry.Message, GetSeverityLevel(logLevel), PopulateLogAttributes(logEntry));
        }

        private static Dictionary<string, string> PopulateLogAttributes(LogEntry logEntry = null)
        {
            var attributes = new Dictionary<string, string>
            {
                {DefaultValues.TypeNameKey , nameof(T) }
            };
            if (logEntry?.CustomLogAttributes != null)
            {
                attributes = logEntry.CustomLogAttributes as Dictionary<string, string>;
                attributes?.Add(DefaultValues.CorrelationId, logEntry.CorrelationId);
                attributes?.Add(DefaultValues.CallerMethodName, logEntry.CallerMethodName);
            }
            return attributes;
        }

        private static SeverityLevel GetSeverityLevel(LogLevel logLevel)
        {
            switch (logLevel)
            {
                case LogLevel.Critical:
                    return SeverityLevel.Critical;
                case LogLevel.Error:
                    return SeverityLevel.Error;
                case LogLevel.Warning:
                    return SeverityLevel.Warning;
                case LogLevel.Information:
                    return SeverityLevel.Information;
                default:
                    return SeverityLevel.Verbose;
            }
        }

        #endregion
    }
}
