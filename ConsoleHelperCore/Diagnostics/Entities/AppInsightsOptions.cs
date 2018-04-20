using System;
using System.Collections.Generic;
using System.Text;

namespace ConsoleHelperCore.Diagnostics.Entities
{
    /// <summary>
    /// AppInsightsOptions class 
    /// </summary>
    public class AppInsightsOptions : LogOptions
    {
        /// <summary>
        /// Instrumentation Key of the App Insights Repo
        /// </summary>
        public string InstrumentationKey { get; set; }
    }
}
