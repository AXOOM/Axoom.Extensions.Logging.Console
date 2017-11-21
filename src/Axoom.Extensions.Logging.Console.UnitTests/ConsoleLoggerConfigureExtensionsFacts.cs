using System;
using System.Linq;
using Axoom.Extensions.Logging.Console.LayoutRenderers;
using Axoom.Extensions.Logging.Console.Layouts;
using FluentAssertions;
using Microsoft.Extensions.Logging;
using Moq;
using Newtonsoft.Json;
using NLog;
using NLog.Config;
using NLog.Extensions.Logging;
using NLog.Targets;
using Xunit;
using ILogger = Microsoft.Extensions.Logging.ILogger;

namespace Axoom.Extensions.Logging.Console
{
    public class ConsoleLoggerConfigureExtensionsFacts
    {
        private readonly LoggerFactory _loggerFactory;

        public ConsoleLoggerConfigureExtensionsFacts()
        {
            _loggerFactory = new LoggerFactory();
        }
        
        [Fact]
        public void AddingConsoleRegistersSysLogLevelLayoutRenderer()
        {
            _loggerFactory.AddConsole();

            ConfigurationItemFactory.Default.LayoutRenderers.TryGetDefinition("sysloglevel", out Type type);
            
            type.ShouldBeEquivalentTo(typeof(SysLogLevelLayoutRenderer));
        }
        
        [Fact]
        public void AddingConsoleRegistersUnixTimeLayoutRenderer()
        {
            _loggerFactory.AddConsole();

            ConfigurationItemFactory.Default.LayoutRenderers.TryGetDefinition("unixtime", out Type type);
            
            type.ShouldBeEquivalentTo(typeof(UnixTimeLayoutRenderer));
        }

        [Fact]
        public void AddingConsoleThrowsArgumentNullException()
        {
            Action addingWithNullFactory = () => ConsoleLoggerConfigureExtensions.AddConsole(factory: null, options: new ConsoleLoggerOptions());
            addingWithNullFactory.ShouldThrow<ArgumentNullException>();
            
            Action addingWithNullOptions = () => _loggerFactory.AddConsole(options: null);
            addingWithNullOptions.ShouldThrow<ArgumentNullException>();
        }

        [Fact]
        public void AddingConsoleRegistersNLogProvider()
        {
            var factoryMock = new Mock<ILoggerFactory>();
            ILoggerFactory factory = factoryMock.Object;

            factory.AddConsole();
            
            factoryMock.Verify(mock => mock.AddProvider(It.IsAny<NLogLoggerProvider>()));
        }

        [Fact]
        public void AddingConsoleHidesOwnCallsite()
        {
            ILoggerFactory loggerFactory = new LoggerFactory().AddConsole();
            var memoryTarget = new MemoryTarget("test-target") {Layout = new GelfLayout()};
            LogManager.Configuration.AddTarget(memoryTarget);
            LogManager.Configuration.AddRuleForAllLevels(memoryTarget);
            LogManager.ReconfigExistingLoggers();
            ILogger logger = loggerFactory.CreateLogger("test");
            
            logger.LogInformation(new Exception(), "test");
            LogManager.Flush();
            
            var logLine = JsonConvert.DeserializeObject<GelfFormat>(memoryTarget.Logs.First());

            logLine.Callsite.Should().Be($"{nameof(ConsoleLoggerConfigureExtensionsFacts)}.{nameof(AddingConsoleHidesOwnCallsite)}");
        }

        private class GelfFormat
        {
            [JsonProperty("_callsite")]
            public string Callsite { get; set; }
        }
    }
}