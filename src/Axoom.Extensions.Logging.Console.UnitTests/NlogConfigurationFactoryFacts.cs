using System.Collections.Generic;
using System.Linq;
using Axoom.Extensions.Logging.Console.Layouts;
using FluentAssertions;
using NLog;
using NLog.Config;
using NLog.Targets;
using NLog.Targets.Wrappers;
using Xunit;

namespace Axoom.Extensions.Logging.Console
{
    public class NlogConfigurationFactoryFacts
    {
        private readonly NlogConfigurationFactory _factory;

        public NlogConfigurationFactoryFacts() 
            => _factory = new NlogConfigurationFactory();

        [Fact]
        public void CreatingWithGelfFormatSetsGelfLayout()
        {
            ConsoleLoggerOptions options = CreateLoggerOptions(LogFormat.Gelf, async: false);

            LoggingConfiguration config = _factory.Create(options);

            ColoredConsoleTarget target = config.AllTargets.OfType<ColoredConsoleTarget>().First();
            target.Layout.Should().BeOfType<GelfLayout>();
        }

        [Fact]
        public void CreatingWithPlainFormatSetsPlainLayout()
        {
            ConsoleLoggerOptions options = CreateLoggerOptions(LogFormat.Plain, async: false);

            LoggingConfiguration config = _factory.Create(options);

            ColoredConsoleTarget target = config.AllTargets.OfType<ColoredConsoleTarget>().First();
            target.Layout.Should().BeOfType<PlainLayout>();
        }

        [Fact]
        public void CreatingWithAsyncCreatesAsyncTargetWrapper()
        {
            ConsoleLoggerOptions options = CreateLoggerOptions(LogFormat.Plain, async: true);

            LoggingConfiguration config = _factory.Create(options);

            config.AllTargets.First().Should().BeOfType<AsyncTargetWrapper>();
        }

        [Fact]
        public void CreatingWithoutAsyncCreatesColoredConsoleTarget()
        {
            ConsoleLoggerOptions options = CreateLoggerOptions(LogFormat.Plain, async: false);

            LoggingConfiguration config = _factory.Create(options);

            config.AllTargets.First().Should().BeOfType<ColoredConsoleTarget>();
        }

        [Fact]
        public void CreatingConfigAddsRuleFromTraceToFatal()
        {
            LoggingConfiguration config = _factory.Create(new ConsoleLoggerOptions());

            LoggingRule loggingRule = config.LoggingRules.First();
            loggingRule.Levels.Should()
                       .Contain(new List<LogLevel>
                       {
                           LogLevel.Trace,
                           LogLevel.Debug,
                           LogLevel.Info,
                           LogLevel.Warn,
                           LogLevel.Error,
                           LogLevel.Fatal
                       });
        }

        private static ConsoleLoggerOptions CreateLoggerOptions(LogFormat format, bool async)
            => new ConsoleLoggerOptions
            {
                Format = format,
                Async = async
            };
    }
}