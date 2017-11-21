using System;
using FluentAssertions;
using NLog;
using Xunit;

namespace Axoom.Extensions.Logging.Console.Layouts
{
    public class PlainLayoutFacts
    {
        private const string LOGGER_NAME = "MyLogger";
        private const string LOG_MESSAGE = "My message";
        private readonly PlainLayout _layout;

        public PlainLayoutFacts() => _layout = new PlainLayout();

        [Fact]
        public void PlainLayoutRendersCorrect()
        {
            var logEventInfo = new LogEventInfo(LogLevel.Debug, LOGGER_NAME, LOG_MESSAGE)
            {
                TimeStamp = DateTime.Now
            };
            
            string output = _layout.Render(logEventInfo);

            output.ShouldBeEquivalentTo(
                $"{logEventInfo.TimeStamp.ToUniversalTime():O} | {logEventInfo.Level.Name.ToUpperInvariant().PadRight(5)} | {logEventInfo.LoggerName} |  | {logEventInfo.FormattedMessage}");
        }

        [Fact]
        public void LogLevelTraceIsCorrectlyPadded()
        {
            var logEventInfo = new LogEventInfo(LogLevel.Trace, LOGGER_NAME, LOG_MESSAGE)
            {
                TimeStamp = DateTime.Now
            };
            
            string output = _layout.Render(logEventInfo);

            output.ShouldBeEquivalentTo(
                $"{logEventInfo.TimeStamp.ToUniversalTime():O} | {logEventInfo.Level.Name.ToUpperInvariant().PadRight(5)} | {logEventInfo.LoggerName} |  | {logEventInfo.FormattedMessage}");
        }
        
        [Fact]
        public void LogLevelDebugIsCorrectlyPadded()
        {
            var logEventInfo = new LogEventInfo(LogLevel.Debug, LOGGER_NAME, LOG_MESSAGE)
            {
                TimeStamp = DateTime.Now
            };
            
            string output = _layout.Render(logEventInfo);

            output.ShouldBeEquivalentTo(
                $"{logEventInfo.TimeStamp.ToUniversalTime():O} | {logEventInfo.Level.Name.ToUpperInvariant().PadRight(5)} | {logEventInfo.LoggerName} |  | {logEventInfo.FormattedMessage}");
        }
        
        [Fact]
        public void LogLevelInfoIsCorrectlyPadded()
        {
            var logEventInfo = new LogEventInfo(LogLevel.Info, LOGGER_NAME, LOG_MESSAGE)
            {
                TimeStamp = DateTime.Now
            };
            
            string output = _layout.Render(logEventInfo);

            output.ShouldBeEquivalentTo(
                $"{logEventInfo.TimeStamp.ToUniversalTime():O} | {logEventInfo.Level.Name.ToUpperInvariant().PadRight(5)} | {logEventInfo.LoggerName} |  | {logEventInfo.FormattedMessage}");
        }
        
        [Fact]
        public void LogLevelWarnIsCorrectlyPadded()
        {
            var logEventInfo = new LogEventInfo(LogLevel.Warn, LOGGER_NAME, LOG_MESSAGE)
            {
                TimeStamp = DateTime.Now
            };
            
            string output = _layout.Render(logEventInfo);

            output.ShouldBeEquivalentTo(
                $"{logEventInfo.TimeStamp.ToUniversalTime():O} | {logEventInfo.Level.Name.ToUpperInvariant().PadRight(5)} | {logEventInfo.LoggerName} |  | {logEventInfo.FormattedMessage}");
        }
        
        [Fact]
        public void LogLevelErrorIsCorrectlyPadded()
        {
            var logEventInfo = new LogEventInfo(LogLevel.Error, LOGGER_NAME, LOG_MESSAGE)
            {
                TimeStamp = DateTime.Now
            };
            
            string output = _layout.Render(logEventInfo);

            output.ShouldBeEquivalentTo(
                $"{logEventInfo.TimeStamp.ToUniversalTime():O} | {logEventInfo.Level.Name.ToUpperInvariant().PadRight(5)} | {logEventInfo.LoggerName} |  | {logEventInfo.FormattedMessage}");
        }
        
        [Fact]
        public void LogLevelFatalIsCorrectlyPadded()
        {
            var logEventInfo = new LogEventInfo(LogLevel.Fatal, LOGGER_NAME, LOG_MESSAGE)
            {
                TimeStamp = DateTime.Now
            };
            
            string output = _layout.Render(logEventInfo);

            output.ShouldBeEquivalentTo(
                $"{logEventInfo.TimeStamp.ToUniversalTime():O} | {logEventInfo.Level.Name.ToUpperInvariant().PadRight(5)} | {logEventInfo.LoggerName} |  | {logEventInfo.FormattedMessage}");
        }
    }
}