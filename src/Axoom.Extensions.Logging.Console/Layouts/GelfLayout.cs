using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using NLog;
using NLog.Layouts;

namespace Axoom.Extensions.Logging.Console.Layouts
{
    internal class GelfLayout : JsonLayout
    {
        public GelfLayout()
        {
            IncludeMdc = false;
            IncludeMdlc = false;
            IncludeAllProperties = false;
            RenderEmptyObject = false;
            SuppressSpaces = true;
            
            Attributes.Add(new JsonAttribute("version", "1.1"));
            Attributes.Add(new JsonAttribute("timestamp", LayoutFormats.TIMESTAMP_UNIX, encode: false));
            Attributes.Add(new JsonAttribute("host", Environment.MachineName));
            Attributes.Add(new JsonAttribute("short_message", LayoutFormats.MESSAGE));
            Attributes.Add(new JsonAttribute("full_message", LayoutFormats.MESSAGE_WITH_EXCEPTION));
            Attributes.Add(new JsonAttribute("level", LayoutFormats.SYS_LOGLEVEL, encode: false));
            Attributes.Add(new JsonAttribute("_timestamp_iso_8601", LayoutFormats.TIMESTAMP_ISO8601));
            Attributes.Add(new JsonAttribute("_loglevel", LayoutFormats.LOGLEVEL));
            Attributes.Add(new JsonAttribute("_logger", LayoutFormats.LOGGER));
            Attributes.Add(new JsonAttribute("_callsite", LayoutFormats.CALLSITE));
            Attributes.Add(new JsonAttribute("_exception_type", LayoutFormats.EXCEPTION_TYPE));
            Attributes.Add(new JsonAttribute("_exception_message", LayoutFormats.EXCEPTION_MESSAGE));
            Attributes.Add(new JsonAttribute("_exception_stacktrace", LayoutFormats.EXCEPTION_STACKTRACE, encode: true));
        }

        protected override void RenderFormattedMessage(LogEventInfo logEvent, StringBuilder target)
        {
            base.RenderFormattedMessage(logEvent, target);
            AddAdditionalFields(target);
        }

        private static void AddAdditionalFields(StringBuilder target)
        {
            JObject jObject = JObject.Parse(target.ToString());
            foreach ((string fieldName, object value) in GetScopeFields())
            {
                jObject.Add(fieldName, JToken.FromObject(value));
            }

            DeduplicateMessage(jObject);

            target.Clear();
            target.Append(jObject.ToString(Formatting.None));
        }

        private static void DeduplicateMessage(JObject jObject)
        {
            JToken shortMessage = jObject.SelectToken("short_message");
            JToken fullMessage = jObject.SelectToken("full_message");
            
            if (shortMessage.Value<string>().Equals(fullMessage.Value<string>()))
                jObject.Remove(fullMessage.Path);
        }

        private static Dictionary<string, object> GetScopeFields()
        {
            var additionalFields = new Dictionary<string, object>();

            PopulateThreadLocalFields(additionalFields);
            PopulateAsyncLocalFields(additionalFields);

            return additionalFields;
        }

        private static void PopulateThreadLocalFields(IDictionary<string, object> additionalFields)
        {
            foreach (string fieldName in MappedDiagnosticsLogicalContext.GetNames())
            {
                additionalFields[fieldName.ToSnakeCase().EnsureLeadingUnderscore()] = MappedDiagnosticsLogicalContext.GetObject(fieldName);
            }
        }

        private static void PopulateAsyncLocalFields(IDictionary<string, object> additionalFields)
        {
            foreach (Dictionary<string, object> dict in NestedDiagnosticsLogicalContext.GetAllObjects().OfType<Dictionary<string, object>>())
            foreach ((string fieldName, object value) in dict)
            {
                additionalFields[fieldName.ToSnakeCase().EnsureLeadingUnderscore()] = value;
            }
        }
    }
}