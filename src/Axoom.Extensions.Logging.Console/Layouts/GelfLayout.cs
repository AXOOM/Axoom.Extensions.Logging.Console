using System;
using System.Collections.Generic;
using System.Text;
using NLog;
using NLog.Layouts;

namespace Axoom.Extensions.Logging.Console.Layouts
{
    internal class GelfLayout : JsonLayout
    {
        public GelfLayout()
        {
            IncludeMdlc = true;
            Attributes.Add(new JsonAttribute("version", "1.1"));
            Attributes.Add(new JsonAttribute("host", Environment.MachineName));
            Attributes.Add(new JsonAttribute("short_message", LayoutFormats.MESSAGE));
            Attributes.Add(new JsonAttribute("full_message", LayoutFormats.MESSAGE_WITH_EXCEPTION));
            Attributes.Add(new JsonAttribute("timestamp", LayoutFormats.TIMESTAMP_UNIX, encode: false));
            Attributes.Add(new JsonAttribute("level", LayoutFormats.SYS_LOGLEVEL, encode: false));
            Attributes.Add(new JsonAttribute("_timestamp", LayoutFormats.TIMESTAMP));
            Attributes.Add(new JsonAttribute("_loglevel", LayoutFormats.LOGLEVEL));
            Attributes.Add(new JsonAttribute("_logger", LayoutFormats.LOGGER));
            Attributes.Add(new JsonAttribute("_callsite", LayoutFormats.CALLSITE));
            Attributes.Add(new JsonAttribute("_exception_type", LayoutFormats.EXCEPTION_TYPE));
            Attributes.Add(new JsonAttribute("_exception_message", LayoutFormats.EXCEPTION_MESSAGE));
            Attributes.Add(new JsonAttribute("_exception_stacktrace", LayoutFormats.EXCEPTION_STACKTRACE, encode: true));
        }

        protected override void RenderFormattedMessage(LogEventInfo logEvent, StringBuilder target)
        {
            EnforceSnakeCasedAdditionalFieldNames();
            base.RenderFormattedMessage(logEvent, target);
        }

        private static void EnforceSnakeCasedAdditionalFieldNames()
        {
            ICollection<string> fieldNames = MappedDiagnosticsLogicalContext.GetNames();
            foreach (string fieldName in fieldNames)
            {
                string newFieldName = fieldName.ToSnakeCase();

                if (newFieldName.Equals(fieldName))
                    return;
                
                RewriteFieldName(fieldName, newFieldName);
            }
        }

        private static void RewriteFieldName(string fieldName, string newFieldName)
        {
            object fieldValue = MappedDiagnosticsLogicalContext.GetObject(fieldName);
            MappedDiagnosticsLogicalContext.Remove(fieldName);
            MappedDiagnosticsLogicalContext.Set(newFieldName, fieldValue);
        }
    }
}