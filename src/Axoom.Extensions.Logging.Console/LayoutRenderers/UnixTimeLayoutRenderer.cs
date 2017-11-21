using System;
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
        protected override void Append(StringBuilder builder, LogEventInfo logEvent)
        {
            DateTime dateTime = logEvent.TimeStamp;
            if (UniversalTime)
                dateTime = dateTime.ToUniversalTime();

            builder.Append(((DateTimeOffset) dateTime).ToUnixTimeSeconds());
        }
    }
}