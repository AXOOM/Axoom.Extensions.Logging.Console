using System.Collections.Generic;
using System.Text;
using JetBrains.Annotations;
using NLog;
using NLog.Config;
using NLog.LayoutRenderers;

namespace Axoom.Extensions.Logging.Console.LayoutRenderers
{
    /// <summary>
    /// Maps NLog's <see cref="LogLevel"/> to SysLog severity (see https://tools.ietf.org/html/rfc5424#section-6.2.1)
    /// </summary>
    [LayoutRenderer("sysloglevel")]
    [ThreadAgnostic]
    [UsedImplicitly]
    internal class SysLogLevelLayoutRenderer : LayoutRenderer
    {
        internal const int SYSLOG_DEBUG = 7;
        internal const int SYSLOG_INFORMATIONAL = 6;
        internal const int SYSLOG_WARNING = 4;
        internal const int SYSLOG_ERROR = 3;
        internal const int SYSLOG_CRITICAL = 2;

        private static readonly Dictionary<LogLevel, int> _mappings = new Dictionary<LogLevel, int>
        {
            {LogLevel.Trace, SYSLOG_DEBUG},
            {LogLevel.Debug, SYSLOG_DEBUG},
            {LogLevel.Info, SYSLOG_INFORMATIONAL},
            {LogLevel.Warn, SYSLOG_WARNING},
            {LogLevel.Error, SYSLOG_ERROR},
            {LogLevel.Fatal, SYSLOG_CRITICAL}
        };

        protected override void Append(StringBuilder builder, LogEventInfo logEvent) => builder.Append(_mappings[logEvent.Level]);
    }
}