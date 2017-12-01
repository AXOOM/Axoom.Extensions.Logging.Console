using System;
using System.Collections.Generic;
using FluentAssertions;
using Microsoft.Extensions.Logging;
using Moq;
using Xunit;

namespace Axoom.Extensions.Logging.Console
{
    public class LoggerExtensionsFacts
    {
        private readonly Mock<ILogger> _loggerMock;
        private readonly ILogger _logger;

        public LoggerExtensionsFacts()
        {
            _loggerMock = new Mock<ILogger>();
            _logger = _loggerMock.Object;
        }

        [Fact]
        public void LogTraceLogsWithEventIdZero()
        {
            _logger.LogTrace(new InvalidOperationException(), "message");

            VerifyLog(LogLevel.Trace);
        }

        [Fact]
        public void LogDebugLogsWithEventIdZero()
        {
            _logger.LogDebug(new InvalidOperationException(), "message");

            VerifyLog(LogLevel.Debug);
        }

        [Fact]
        public void LogInformationLogsWithEventIdZero()
        {
            _logger.LogInformation(new InvalidOperationException(), "message");

            VerifyLog(LogLevel.Information);
        }

        [Fact]
        public void LogWarningLogsWithEventIdZero()
        {
            _logger.LogWarning(new InvalidOperationException(), "message");

            VerifyLog(LogLevel.Warning);
        }

        [Fact]
        public void LogErrorLogsWithEventIdZero()
        {
            _logger.LogError(new InvalidOperationException(), "message");

            VerifyLog(LogLevel.Error);
        }

        [Fact]
        public void LogCriticalLogsWithEventIdZero()
        {
            _logger.LogCritical(new InvalidOperationException(), "message");

            VerifyLog(LogLevel.Critical);
        }

        private void VerifyLog(LogLevel logLevel)
        {
            _loggerMock.Verify(mock => mock.Log(logLevel,
                                                0,
                                                It.IsAny<object>(),
                                                It.IsAny<InvalidOperationException>(),
                                                It.IsAny<Func<object, Exception, string>>()));
        }
    }
}