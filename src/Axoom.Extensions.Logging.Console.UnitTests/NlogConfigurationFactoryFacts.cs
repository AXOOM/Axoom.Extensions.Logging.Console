using System.Collections.Generic;
using System.Linq;
using Axoom.Extensions.Logging.Console.Layouts;
using FluentAssertions;
using NLog;
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
            var options = CreateLoggerOptions(LogFormat.Gelf, async: false);

            var config = _factory.Create(options);

            var target = config.AllTargets.OfType<ColoredConsoleTarget>().First();
            target.Layout.Should().BeOfType<GelfLayout>();
        }

        [Fact]
        public void CreatingWithPlainFormatSetsPlainLayout()
        {
            var options = CreateLoggerOptions(LogFormat.Plain, async: false);

            var config = _factory.Create(options);

            var target = config.AllTargets.OfType<ColoredConsoleTarget>().First();
            target.Layout.Should().BeOfType<PlainLayout>();
        }

        [Fact]
        public void CreatingWithAsyncCreatesAsyncTargetWrapper()
        {
            var options = CreateLoggerOptions(LogFormat.Plain, async: true);

            var config = _factory.Create(options);

            config.AllTargets.First().Should().BeOfType<AsyncTargetWrapper>();
        }

        [Fact]
        public void CreatingWithoutAsyncCreatesColoredConsoleTarget()
        {
            var options = CreateLoggerOptions(LogFormat.Plain, async: false);

            var config = _factory.Create(options);

            config.AllTargets.First().Should().BeOfType<ColoredConsoleTarget>();
        }

        [Fact]
        public void CreatingConfigAddsRuleFromTraceToFatal()
        {
            var config = _factory.Create(new ConsoleLoggerOptions());

            var loggingRule = config.LoggingRules.First();
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