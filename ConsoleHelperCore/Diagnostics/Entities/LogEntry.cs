using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Text;

namespace ConsoleHelperCore.Diagnostics.Entities
{
    /// <summary>
    /// Log detail entity 
    /// </summary>
    public class LogEntry
    {
        public LogEntry([CallerMemberName] string caller = "")
        {
            CallerMethodName = caller;
        }
        /// <summary>
        /// Log Message
        /// </summary>
        public string Message { get; set; }
        /// <summary>
        /// A unique identifier helpful in tracing your log(s)
        /// </summary>
        public string CorrelationId { get; set; }
        /// <summary>
        /// Additional attributes customized for this log entry
        /// </summary>
        public IDictionary<string, string> CustomLogAttributes { get; set; }
        /// <summary>
        /// Instantiator of this class
        /// </summary>
        internal string CallerMethodName { get; }
    }
}
