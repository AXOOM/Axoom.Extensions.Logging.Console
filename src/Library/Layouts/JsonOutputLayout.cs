using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NLog;
using NLog.Config;
using NLog.Layouts;

namespace Axoom.Extensions.Logging.Console.Layouts
{
    [ThreadAgnostic]
    [ThreadSafe]
    public class JsonOutputLayout : JsonLayout
    {
        public JsonOutputLayout()
        {
            IncludeMdc = true;
            IncludeMdlc = true;
            IncludeAllProperties = false;
            RenderEmptyObject = false;
            SuppressSpaces = true;

            Attributes.Add(new JsonAttribute("message", LayoutFormats.MESSAGE));
            Attributes.Add(new JsonAttribute("timestamp_unix", LayoutFormats.TIMESTAMP_UNIX, encode: false));
            Attributes.Add(new JsonAttribute("timestamp_iso8601", LayoutFormats.TIMESTAMP_ISO8601));
            Attributes.Add(new JsonAttribute("loglevel", LayoutFormats.LOGLEVEL));
            Attributes.Add(new JsonAttribute("syslog_level", LayoutFormats.SYS_LOGLEVEL, encode: false));
            Attributes.Add(new JsonAttribute("logger", LayoutFormats.LOGGER));
            Attributes.Add(new JsonAttribute("callsite", LayoutFormats.CALLSITE));
            Attributes.Add(new JsonAttribute("exception_type", LayoutFormats.EXCEPTION_TYPE));
            Attributes.Add(new JsonAttribute("exception_message", LayoutFormats.EXCEPTION_MESSAGE));
            Attributes.Add(new JsonAttribute("exception_stacktrace", LayoutFormats.EXCEPTION_STACKTRACE, encode: true));
        }
    }
}
