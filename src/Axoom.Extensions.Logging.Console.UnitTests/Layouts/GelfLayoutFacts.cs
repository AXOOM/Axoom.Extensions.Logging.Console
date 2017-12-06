using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using Axoom.Extensions.Logging.Console.LayoutRenderers;
using FluentAssertions;
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
    public class GelfLayoutFacts
    {
        private readonly GelfLayout _layout;
        private readonly LogEventInfo _logEventInfo;

        public GelfLayoutFacts()
        {
            new LoggerFactory().AddAxoomConsole();

            _layout = new GelfLayout();
            _logEventInfo = new LogEventInfo(LogLevel.Debug, "MyLogger", "MyMessage")
            {
                TimeStamp = DateTime.Now
            };
        }

        [Fact]
        public void OutputDoesNotContainAdditionalFieldId()
        {
            string output = _layout.Render(_logEventInfo);

            var gelfMessage = JsonConvert.DeserializeObject<JObject>(output);
            gelfMessage.Properties().Select(prop => prop.Name).Should().NotContain("_id");
        }

        [Fact]
        public void OutputContainsVersion11()
        {
            string output = _layout.Render(_logEventInfo);

            JProperty property = GetProperty(output, "version");
            property.Value.Value<string>().ShouldBeEquivalentTo("1.1");
        }

        [Fact]
        public void OutputContainsHost()
        {
            string output = _layout.Render(_logEventInfo);

            JProperty property = GetProperty(output, "host");
            property.Value.Value<string>().Should().NotBeNullOrEmpty();
        }

        [Fact]
        public void OutputContainsShortMessage()
        {
            string output = _layout.Render(_logEventInfo);

            JProperty property = GetProperty(output, "short_message");
            property.Value.Value<string>().Should().NotBeNullOrEmpty();
        }

        [Fact]
        public void OutputContainsTimestamp()
        {
            string output = _layout.Render(_logEventInfo);

            JProperty property = GetProperty(output, "timestamp");
            property.Value.Value<decimal>().Should().BeGreaterThan(0);
        }

        [Fact]
        public void OutputContainsLevel()
        {
            string output = _layout.Render(_logEventInfo);

            JProperty property = GetProperty(output, "level");
            property.Value.Value<int>()
                    .Should()
                    .BeOneOf(SysLogLevelLayoutRenderer.SYSLOG_CRITICAL,
                             SysLogLevelLayoutRenderer.SYSLOG_DEBUG,
                             SysLogLevelLayoutRenderer.SYSLOG_ERROR,
                             SysLogLevelLayoutRenderer.SYSLOG_INFORMATIONAL,
                             SysLogLevelLayoutRenderer.SYSLOG_WARNING);
        }

        [Fact]
        public void AdditionalFieldsArePrefixedWithUnderscore()
        {
            string output = _layout.Render(_logEventInfo);

            var gelfMessage = JsonConvert.DeserializeObject<JObject>(output);
            gelfMessage.Remove("version");
            gelfMessage.Remove("host");
            gelfMessage.Remove("short_message");
            gelfMessage.Remove("full_message");
            gelfMessage.Remove("timestamp");
            gelfMessage.Remove("level");
            gelfMessage.Remove("facility");
            gelfMessage.Remove("line");
            gelfMessage.Remove("file");

            gelfMessage.Properties().ToList().ForEach(prop => prop.Name.Should().StartWith("_", "additional fields have to start with an underscore"));
        }

        [Fact]
        public void AdditionalFieldsHaveValidNames()
        {
            var regex = new Regex(@"^[\w\-]*$");

            string output = _layout.Render(_logEventInfo);

            var gelfMessage = JsonConvert.DeserializeObject<JObject>(output);
            gelfMessage.Properties()
                       .ToList()
                       .ForEach(prop => prop.Name.Should().MatchRegex(regex.ToString(), "additional fields have to match the regex pattern"));
        }

        [Fact]
        public void AddedScopeFieldsAreWrittenToGelf()
        {
            var gelfLayout = new GelfLayout();
            MappedDiagnosticsLogicalContext.Set("_test_field1", 4711);
            MappedDiagnosticsLogicalContext.Set("_test_field2", "foo");

            string renderedGelf = gelfLayout.Render(new LogEventInfo(LogLevel.Info, "logger", "message"));

            var logLine = JsonConvert.DeserializeObject<JObject>(renderedGelf);
            logLine.SelectToken("_test_field1").Value<int>().Should().Be(4711);
            logLine.SelectToken("_test_field2").Value<string>().Should().Be("foo");
        }

        [Fact]
        public void AddedScopeFieldNamesAreConvertedToSnakeCase()
        {
            var gelfLayout = new GelfLayout();
            MappedDiagnosticsLogicalContext.Set("AField", 4711);
            MappedDiagnosticsLogicalContext.Set("Another Field", 4711);
            MappedDiagnosticsLogicalContext.Set("missing_leading_underscore", 4711);

            string renderedGelf = gelfLayout.Render(new LogEventInfo(LogLevel.Info, "logger", "message"));

            var logLine = JsonConvert.DeserializeObject<JObject>(renderedGelf);
            logLine.SelectToken("_a_field").Should().NotBeNull();
            logLine.SelectToken("_another_field").Should().NotBeNull();
            logLine.SelectToken("_missing_leading_underscore").Should().NotBeNull();
        }

        [Fact]
        public void StringObjectDictionaryScopeIsSupported()
        {
            ILoggerFactory factory = new LoggerFactory().AddAxoomConsole(new ConsoleLoggerOptions {Async = false, IncludeScopes = true});

            var debugTarget = new DebugTarget("debug") {Layout = new GelfLayout()};
            LogManager.Configuration.AddTarget(debugTarget);
            LogManager.Configuration.AddRuleForOneLevel(LogLevel.Info, debugTarget);
            LogManager.ReconfigExistingLoggers();
            ILogger logger = factory.CreateLogger("test");

            using (logger.BeginScope(new Dictionary<string, object> {{"myField", "value"}}))
            {
                logger.LogInformation("test");
            }

            debugTarget.LastMessage.Should().Contain("\"_my_field\":\"value\"");
        }

        private static JProperty GetProperty(string output, string propertyName)
        {
            var gelfMessage = JsonConvert.DeserializeObject<JObject>(output);
            JProperty property = gelfMessage.Properties().FirstOrDefault(prop => prop.Name.Equals(propertyName));
            property.Should().NotBeNull();
            return property;
        }
    }
}