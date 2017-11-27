using System;
using JetBrains.Annotations;
using Microsoft.Extensions.Configuration;

namespace Axoom.Extensions.Logging.Console
{
    [PublicAPI]
    public class ConsoleLoggerOptions
    {
        public ConsoleLoggerOptions()
        {
        }

        public ConsoleLoggerOptions(IConfiguration configuration)
        {
            Format = ReadLogFormatOption(configuration);
            Async = ReadAsyncOption(configuration);
            IncludeScopes = ReadIncludeScopesOption(configuration);
        }

        public LogFormat Format { get; set; } = LogFormat.Gelf;
        public bool Async { get; set; } = true;
        public bool IncludeScopes { get; set; }

        private bool ReadAsyncOption(IConfiguration configuration)
        {
            string value = configuration["Async"];

            if (string.IsNullOrEmpty(value))
                return new ConsoleLoggerOptions().Async;

            bool.TryParse(value, out bool logAsync);
            return logAsync;
        }

        private LogFormat ReadLogFormatOption(IConfiguration configuration)
        {
            string value = configuration["Format"];
            if (string.IsNullOrEmpty(value))
                return new ConsoleLoggerOptions().Format;

            if (Enum.TryParse(value, out LogFormat format) && Enum.IsDefined(typeof(LogFormat), format))
                return format;

            throw new InvalidOperationException($"Configuration \"{value}\" for setting \"{nameof(Format)}\" is not supported.");
        }

        private bool ReadIncludeScopesOption(IConfiguration configuration)
        {
            string value = configuration["IncludeScopes"];

            if (string.IsNullOrEmpty(value))
                return new ConsoleLoggerOptions().IncludeScopes;

            bool.TryParse(value, out bool includeScopes);
            return includeScopes;
        }
    }
}