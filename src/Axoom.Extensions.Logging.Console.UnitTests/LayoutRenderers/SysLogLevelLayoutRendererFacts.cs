using FluentAssertions;
using NLog;
using Xunit;

namespace Axoom.Extensions.Logging.Console.LayoutRenderers
{
    public class SysLogLevelLayoutRendererFacts
    {
        private readonly SysLogLevelLayoutRenderer _layoutRenderer;

        public SysLogLevelLayoutRendererFacts()
        {
            _layoutRenderer = new SysLogLevelLayoutRenderer();
        }
        
        [Fact]
        public void TraceIsMappedToSyslogDebug()
        {
            string output = _layoutRenderer.Render(CreateLogEventInfo(LogLevel.Trace));
            
            output.ShouldBeEquivalentTo(SysLogLevelLayoutRenderer.SYSLOG_DEBUG);
        }
        
        [Fact]
        public void DebugIsMappedToSyslogDebug()
        {
            string output = _layoutRenderer.Render(CreateLogEventInfo(LogLevel.Debug));
            
            output.ShouldBeEquivalentTo("7");
        }
        
        [Fact]
        public void InfoIsMappedToSyslogInformational()
        {
            string output = _layoutRenderer.Render(CreateLogEventInfo(LogLevel.Info));
            
            output.ShouldBeEquivalentTo(SysLogLevelLayoutRenderer.SYSLOG_INFORMATIONAL);
        }
        
        [Fact]
        public void WarnIsMappedToSyslogWarning()
        {
            string output = _layoutRenderer.Render(CreateLogEventInfo(LogLevel.Warn));
            
            output.ShouldBeEquivalentTo(SysLogLevelLayoutRenderer.SYSLOG_WARNING);
        }
        
        [Fact]
        public void ErrorIsMappedToSyslogError()
        {
            string output = _layoutRenderer.Render(CreateLogEventInfo(LogLevel.Error));
            
            output.ShouldBeEquivalentTo(SysLogLevelLayoutRenderer.SYSLOG_ERROR);
        }
        
        [Fact]
        public void FatalIsMappedToSyslogCritical()
        {
            string output = _layoutRenderer.Render(CreateLogEventInfo(LogLevel.Fatal));
            
            output.ShouldBeEquivalentTo(SysLogLevelLayoutRenderer.SYSLOG_CRITICAL);
        }

        private static LogEventInfo CreateLogEventInfo(LogLevel logLevel) => new LogEventInfo(logLevel, "ALogger", "A message");
    }
}