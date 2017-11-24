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
            Format = ReadLogFormat(configuration);
            Async = ReadAsync(configuration);
        }
        
        public LogFormat Format { get; set; } = LogFormat.Gelf;
        public bool Async { get; set; } = true;

        private bool ReadAsync(IConfiguration configuration)
        {
            string value = configuration["Async"];

            if (string.IsNullOrEmpty(value))
                return true;
                
            bool.TryParse(value, out bool async);
            return async;
        }
        
        private LogFormat ReadLogFormat(IConfiguration configuration)
        {
            string value = configuration["Format"];
            if (string.IsNullOrEmpty(value))
                return LogFormat.Gelf;
                
            if (Enum.TryParse(value, out LogFormat format))
            {
                return format;
            }
                
            throw new InvalidOperationException($"Configuration \"{value}\" for setting \"{Format}\" is not supported.");
        }
    }
}