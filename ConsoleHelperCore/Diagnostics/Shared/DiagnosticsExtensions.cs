using System;
using System.Collections.Generic;
using System.Text;
using ConsoleHelperCore.Diagnostics.Entities;
using ConsoleHelperCore.Diagnostics.Interface;
using Microsoft.ApplicationInsights;
using Microsoft.ApplicationInsights.Extensibility;
using SimpleInjector;

namespace ConsoleHelperCore.Diagnostics.Shared
{
    public static class DiagnosticsExtensions
    {
        private static bool _isTelemetryRegistered = false;

        /// <summary>
        /// Adding AppInsights Logger without IApplicationBuilder middleware
        /// </summary>
        /// <param name="container"></param>
        public static Container AddApplicationInsightsUsingSimpleInjector(this Container container)
        {
            if (container == null) throw new ArgumentNullException(nameof(container));

            container.RegisterSingleton(typeof(ILogger<>), typeof(ApplicationInsightsLogger<>));
            return container;
        }

        /// <summary>
        /// Add options like instrumentation key and log levels 
        /// </summary>
        /// <param name="container"></param>
        /// <param name="options"></param>
        /// <returns></returns>
        public static Container AddAppInsightsLoggingOptions(this Container container, AppInsightsOptions options)
        {
            if (options == null) throw new ArgumentNullException(nameof(options));
            if (!_isTelemetryRegistered)
            {
                container.RegisterInstance(new TelemetryClient(new TelemetryConfiguration(options.InstrumentationKey)));
            }
            container.RegisterInstance(typeof(LogOptions), options);
            return container;
        }
    }
}
