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
                return true;

            bool.TryParse(value, out bool async);
            return async;
        }

        private LogFormat ReadLogFormatOption(IConfiguration configuration)
        {
            string value = configuration["Format"];
            if (string.IsNullOrEmpty(value))
                return LogFormat.Gelf;

            if (Enum.TryParse(value, out LogFormat format))
            {
                return format;
            }

            throw new InvalidOperationException($"Configuration \"{value}\" for setting \"{nameof(Format)}\" is not supported.");
        }

        private bool ReadIncludeScopesOption(IConfiguration configuration)
        {
            string value = configuration["IncludeScopes"];

            if (string.IsNullOrEmpty(value))
                return true;

            bool.TryParse(value, out bool includeScopes);
            return includeScopes;
        }
    }
}