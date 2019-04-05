using System;
using System.Collections.Generic;
using FluentAssertions;
using Microsoft.Extensions.Configuration;
using Xunit;

namespace Axoom.Extensions.Logging.Console
{
    public class ConsoleLoggerOptionsFacts
    {
        [Fact]
        public void ReadsAsyncOptionFromConfiguration()
        {
            var configuration = BuildConfiguration(logAsync: false);

            var options = new ConsoleLoggerOptions(configuration);

            options.Async.Should().BeFalse();
        }

        [Fact]
        public void ReadingUnsetAsyncOptionFromConfigurationSetsToDefault()
        {
            IConfiguration configuration = new ConfigurationBuilder().Build();

            var options = new ConsoleLoggerOptions(configuration);

            options.Async.Should().Be(new ConsoleLoggerOptions().Async);
        }

        [Fact]
        public void ReadsLogFormatFromConfiguration()
        {
            var configuration = BuildConfiguration(logFormat: LogFormat.Plain);

            var options = new ConsoleLoggerOptions(configuration);

            options.Format.Should().Be(LogFormat.Plain);
        }

        [Fact]
        public void ReadingUnsetLogFormatFromConfigurationSetsToDefault()
        {
            IConfiguration configuration = new ConfigurationBuilder().Build();

            var options = new ConsoleLoggerOptions(configuration);

            options.Format.Should().Be(new ConsoleLoggerOptions().Format);
        }

        [Fact]
        public void ReadingInvalidLogFormatThrowsInvalidOperationException()
        {
            var configuration = BuildConfiguration(logFormat: (LogFormat) 999);

            Action creatingOptions = () => new ConsoleLoggerOptions(configuration);

            creatingOptions.Should().Throw<InvalidOperationException>();
        }

        [Fact]
        public void ReadsIncludeScopesOptionFromConfiguration()
        {
            var configuration = BuildConfiguration(includeScopes: true);

            var options = new ConsoleLoggerOptions(configuration);

            options.IncludeScopes.Should().BeTrue();
        }

        [Fact]
        public void ReadingUnsetIncludeScopesOptionFromConfigurationSetsToDefault()
        {
            IConfiguration configuration = new ConfigurationBuilder().Build();

            var options = new ConsoleLoggerOptions(configuration);

            options.IncludeScopes.Should().Be(new ConsoleLoggerOptions().IncludeScopes);
        }

        [Fact]
        public void ReadingFormatIsCaseInsensitive()
        {
            var configuration = new ConfigurationBuilder()
                .AddInMemoryCollection(new Dictionary<string, string> {["Format"] = "plain"})
                .Build();

            Action loadingOptions = () => new ConsoleLoggerOptions(configuration);

            loadingOptions.Should().NotThrow<InvalidOperationException>();
        }

        private static IConfiguration BuildConfiguration(LogFormat logFormat = LogFormat.Json, bool logAsync = true, bool includeScopes = false)
            => new ConfigurationBuilder()
                .AddInMemoryCollection(new Dictionary<string, string>
                {
                    {"Format", logFormat.ToString()},
                    {"Async", logAsync.ToString()},
                    {"IncludeScopes", includeScopes.ToString()}
                })
                .Build();
    }
}