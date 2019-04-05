using FluentAssertions;
using NLog;
using Xunit;

namespace Axoom.Extensions.Logging.Console.LayoutRenderers
{
    public class SysLogLevelLayoutRendererFacts
    {
        private readonly SysLogLevelLayoutRenderer _layoutRenderer;

        public SysLogLevelLayoutRendererFacts()
            => _layoutRenderer = new SysLogLevelLayoutRenderer();

        [Fact]
        public void TraceIsMappedToSyslogDebug()
        {
            string output = _layoutRenderer.Render(CreateLogEventInfo(LogLevel.Trace));

            output.Should().Be(SysLogLevelLayoutRenderer.SYSLOG_DEBUG.ToString());
        }

        [Fact]
        public void DebugIsMappedToSyslogDebug()
        {
            string output = _layoutRenderer.Render(CreateLogEventInfo(LogLevel.Debug));

            output.Should().Be("7");
        }

        [Fact]
        public void InfoIsMappedToSyslogInformational()
        {
            string output = _layoutRenderer.Render(CreateLogEventInfo(LogLevel.Info));

            output.Should().Be(SysLogLevelLayoutRenderer.SYSLOG_INFORMATIONAL.ToString());
        }

        [Fact]
        public void WarnIsMappedToSyslogWarning()
        {
            string output = _layoutRenderer.Render(CreateLogEventInfo(LogLevel.Warn));

            output.Should().Be(SysLogLevelLayoutRenderer.SYSLOG_WARNING.ToString());
        }

        [Fact]
        public void ErrorIsMappedToSyslogError()
        {
            string output = _layoutRenderer.Render(CreateLogEventInfo(LogLevel.Error));

            output.Should().Be(SysLogLevelLayoutRenderer.SYSLOG_ERROR.ToString());
        }

        [Fact]
        public void FatalIsMappedToSyslogCritical()
        {
            string output = _layoutRenderer.Render(CreateLogEventInfo(LogLevel.Fatal));

            output.Should().Be(SysLogLevelLayoutRenderer.SYSLOG_CRITICAL.ToString());
        }

        private static LogEventInfo CreateLogEventInfo(LogLevel logLevel) => new LogEventInfo(logLevel, "ALogger", "A message");
    }
}