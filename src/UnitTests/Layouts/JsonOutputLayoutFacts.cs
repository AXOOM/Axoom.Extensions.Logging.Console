using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using Axoom.Extensions.Logging.Console.LayoutRenderers;
using FluentAssertions;
using FluentAssertions.Extensions;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using NLog;
using NLog.Targets;
using Xunit;
using ILogger = Microsoft.Extensions.Logging.ILogger;
using LogLevel = NLog.LogLevel;

namespace Axoom.Extensions.Logging.Console.Layouts
{
    public class JsonOutputLayoutFacts
    {
        private readonly JsonOutputLayout _layout;
        private readonly LogEventInfo _logEventInfo;

        public JsonOutputLayoutFacts()
        {
            new LoggerFactory().AddAxoomConsole();

            _layout = new JsonOutputLayout();
            _logEventInfo = new LogEventInfo(LogLevel.Debug, "MyLogger", "MyMessage")
            {
                TimeStamp = DateTime.Now
            };
        }

        [Fact]
        public void OutputContainsMessage()
        {
            string output = _layout.Render(_logEventInfo);

            var property = GetProperty(output, "message");
            property.Value.Value<string>().Should().NotBeNullOrEmpty();
        }

        [Fact]
        public void OutputContainsUnixTimestamp()
        {
            string output = _layout.Render(_logEventInfo);

            var property = GetProperty(output, "timestamp_unix");
            property.Value.Value<decimal>().Should().BeGreaterThan(0);
        }
        
        [Fact]
        public void OutputContainsIsoTimestamp()
        {
            string output = _layout.Render(_logEventInfo);

            var property = GetProperty(output, "timestamp_iso8601");
            property.Value.Value<DateTime>().Should().Be(_logEventInfo.TimeStamp.ToUniversalTime());
        }

        [Fact]
        public void OutputContainsLevel()
        {
            string output = _layout.Render(_logEventInfo);

            var property = GetProperty(output, "syslog_level");
            property.Value.Value<int>()
                    .Should()
                    .BeOneOf(SysLogLevelLayoutRenderer.SYSLOG_CRITICAL,
                             SysLogLevelLayoutRenderer.SYSLOG_DEBUG,
                             SysLogLevelLayoutRenderer.SYSLOG_ERROR,
                             SysLogLevelLayoutRenderer.SYSLOG_INFORMATIONAL,
                             SysLogLevelLayoutRenderer.SYSLOG_WARNING);
        }

        [Fact]
        public void AdditionalFieldsHaveValidNames()
        {
            var regex = new Regex(@"^[\w\-]*$");

            string output = _layout.Render(_logEventInfo);

            var message = JsonConvert.DeserializeObject<JObject>(output);
            message.Properties()
                       .ToList()
                       .ForEach(prop => prop.Name.Should().MatchRegex(regex.ToString(), "additional fields have to match the regex pattern"));
        }

        [Fact]
        public void AddedScopeFieldsAreWrittenToJson()
        {
            var layout = new JsonOutputLayout();
            MappedDiagnosticsLogicalContext.Set("test_field1", 4711);
            MappedDiagnosticsLogicalContext.Set("test_field2", "foo");

            string rendered = layout.Render(new LogEventInfo(LogLevel.Info, "logger", "message"));

            var logLine = JsonConvert.DeserializeObject<JObject>(rendered);
            logLine.SelectToken("test_field1").Value<int>().Should().Be(4711);
            logLine.SelectToken("test_field2").Value<string>().Should().Be("foo");
        }

        [Fact]
        public void StringObjectDictionaryScopeIsSupported()
        {
            var debugTarget = CreateDebuggableLogger(includeScopes: true, logger: out var logger);

            using (logger.BeginScope(new Dictionary<string, object> {{"myField", "value"}}))
            {
                logger.LogInformation("test");
            }

            debugTarget.LastMessage.Should().Contain("\"myField\":\"value\"");
        }

        private static DebugTarget CreateDebuggableLogger(bool includeScopes, out ILogger logger)
        {
            var factory = new LoggerFactory().AddAxoomConsole(new ConsoleLoggerOptions {Async = false, IncludeScopes = includeScopes});

            var debugTarget = new DebugTarget("debug") {Layout = new JsonOutputLayout()};
            LogManager.Configuration.AddTarget(debugTarget);
            LogManager.Configuration.AddRuleForOneLevel(LogLevel.Info, debugTarget);
            LogManager.ReconfigExistingLoggers();
            logger = factory.CreateLogger("test");
            return debugTarget;
        }

        private static JProperty GetProperty(string output, string propertyName)
        {
            var message = JsonConvert.DeserializeObject<JObject>(output);
            var property = message.Properties().FirstOrDefault(prop => prop.Name.Equals(propertyName));
            property.Should().NotBeNull();
            return property;
        }
    }
}
