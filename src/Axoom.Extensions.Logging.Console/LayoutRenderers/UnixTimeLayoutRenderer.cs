using System;
using System.Globalization;
using System.Text;
using JetBrains.Annotations;
using NLog;
using NLog.Config;
using NLog.LayoutRenderers;

namespace Axoom.Extensions.Logging.Console.LayoutRenderers
{
    [LayoutRenderer("unixtime")]
    [ThreadAgnostic]
    [UsedImplicitly]
    internal class UnixTimeLayoutRenderer : LongDateLayoutRenderer
    {
        private static readonly NumberFormatInfo _numberFormat = new NumberFormatInfo{NumberDecimalSeparator = "."};
        
        protected override void Append(StringBuilder builder, LogEventInfo logEvent)
        {
            DateTime dateTime = logEvent.TimeStamp;
            if (UniversalTime)
                dateTime = dateTime.ToUniversalTime();

            double timestamp = ((DateTimeOffset) dateTime).ToUnixTimeMilliseconds() / 1000d;
            builder.Append(timestamp.ToString(_numberFormat));
        }
    }
}