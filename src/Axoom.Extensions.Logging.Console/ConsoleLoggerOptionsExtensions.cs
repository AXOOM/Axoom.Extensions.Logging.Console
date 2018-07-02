using NLog.Extensions.Logging;

namespace Axoom.Extensions.Logging.Console
{
    internal static class ConsoleLoggerOptionsExtensions
    {
        public static NLogProviderOptions GetNlogProviderOptions(this ConsoleLoggerOptions options)
            => new NLogProviderOptions
            {
                CaptureMessageProperties = options.IncludeScopes
            };
    }
}
