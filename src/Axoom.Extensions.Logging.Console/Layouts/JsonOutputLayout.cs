using System.Collections.Generic;
using System.Linq;
using System.Text;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using NLog;
using NLog.Layouts;

namespace Axoom.Extensions.Logging.Console.Layouts
{
    public class JsonOutputLayout : JsonLayout
    {
        public JsonOutputLayout()
        {
            IncludeMdc = false;
            IncludeMdlc = false;
            IncludeAllProperties = false;
            RenderEmptyObject = false;
            SuppressSpaces = true;

            Attributes.Add(new JsonAttribute("message", LayoutFormats.MESSAGE));
            Attributes.Add(new JsonAttribute("level", LayoutFormats.SYS_LOGLEVEL, encode: false));
            Attributes.Add(new JsonAttribute("timestamp_unix", LayoutFormats.TIMESTAMP_UNIX, encode: false));
            Attributes.Add(new JsonAttribute("timestamp_iso8601", LayoutFormats.TIMESTAMP_ISO8601));
            Attributes.Add(new JsonAttribute("loglevel", LayoutFormats.LOGLEVEL));
            Attributes.Add(new JsonAttribute("logger", LayoutFormats.LOGGER));
            Attributes.Add(new JsonAttribute("callsite", LayoutFormats.CALLSITE));
            Attributes.Add(new JsonAttribute("exception_type", LayoutFormats.EXCEPTION_TYPE));
            Attributes.Add(new JsonAttribute("exception_message", LayoutFormats.EXCEPTION_MESSAGE));
            Attributes.Add(new JsonAttribute("exception_stacktrace", LayoutFormats.EXCEPTION_STACKTRACE, encode: true));
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
                jObject.Add(fieldName.ToSnakeCase(), JToken.FromObject(value));
            }

            target.Clear();
            target.Append(jObject.ToString(Formatting.None));
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
                additionalFields[fieldName.ToSnakeCase()] = MappedDiagnosticsLogicalContext.GetObject(fieldName);
            }
        }

        private static void PopulateAsyncLocalFields(IDictionary<string, object> additionalFields)
        {
            foreach (Dictionary<string, object> dict in NestedDiagnosticsLogicalContext.GetAllObjects().OfType<Dictionary<string, object>>())
            foreach ((string fieldName, object value) in dict)
            {
                additionalFields[fieldName.ToSnakeCase()] = value;
            }
        }
    }
}
