using System;
using System.Collections.Generic;
using System.Reflection;
using Axoom.Extensions.Logging.Console.LayoutRenderers;
using FluentAssertions;
using Microsoft.Extensions.Logging;
using Moq;
using NLog;
using NLog.Config;
using NLog.Extensions.Logging;
using Xunit;
using ILogger = Microsoft.Extensions.Logging.ILogger;

namespace Axoom.Extensions.Logging.Console
{
    public class ConsoleLoggerConfigureExtensionsFacts
    {
        private readonly ILoggerFactory _loggerFactory;

        public ConsoleLoggerConfigureExtensionsFacts() 
            => _loggerFactory = new LoggerFactory().AddAxoomConsole();

        [Fact]
        public void AddingConsoleRegistersSysLogLevelLayoutRenderer()
        {
            ConfigurationItemFactory.Default.LayoutRenderers.TryGetDefinition("sysloglevel", out Type type);

            type.ShouldBeEquivalentTo(typeof(SysLogLevelLayoutRenderer));
        }

        [Fact]
        public void AddingConsoleRegistersUnixTimeLayoutRenderer()
        {
            ConfigurationItemFactory.Default.LayoutRenderers.TryGetDefinition("unixtime", out Type type);

            type.ShouldBeEquivalentTo(typeof(UnixTimeLayoutRenderer));
        }

        [Fact]
        public void AddingConsoleThrowsArgumentNullException()
        {
            Action addingWithNullFactory = () => ConsoleLoggerConfigureExtensions.AddAxoomConsole(factory: null, options: new ConsoleLoggerOptions());
            addingWithNullFactory.ShouldThrow<ArgumentNullException>();

            Action addingWithNullOptions = () => _loggerFactory.AddAxoomConsole(options: null);
            addingWithNullOptions.ShouldThrow<ArgumentNullException>();
        }

        [Fact]
        public void AddingConsoleRegistersNLogProvider()
        {
            var factoryMock = new Mock<ILoggerFactory>();
            ILoggerFactory factory = factoryMock.Object;

            factory.AddAxoomConsole();

            factoryMock.Verify(mock => mock.AddProvider(It.IsAny<NLogLoggerProvider>()));
        }

        [Fact]
        public void AddingConsoleHidesOwnCallsite()
        {
            new LoggerFactory().AddAxoomConsole();

            var hiddenAssemblies =
                (ICollection<Assembly>) typeof(LogManager).GetField("_hiddenAssemblies", BindingFlags.Static | BindingFlags.NonPublic).GetValue(null);
            
            hiddenAssemblies.Should().Contain(Assembly.Load(new AssemblyName("Axoom.Extensions.Logging.Console")));
        }
    }
}