using JetBrains.Annotations;

namespace Axoom.Extensions.Logging.Console
{
    [PublicAPI]
    public class ConsoleLoggerOptions
    {
        public LogFormat Format { get; set; } = LogFormat.Gelf;
        public bool Async { get; set; } = true;
    }
}