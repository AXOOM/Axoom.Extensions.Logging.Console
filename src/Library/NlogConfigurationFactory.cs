using NLog;
using NLog.Config;
using NLog.Targets;
using NLog.Targets.Wrappers;

namespace Axoom.Extensions.Logging.Console
{
    public class NlogConfigurationFactory
    {
        private const string TARGET_CONSOLE = "console";
        private const string TARGET_ASYNC_CONSOLE = "async_console";

        public LoggingConfiguration Create(ConsoleLoggerOptions loggingOptions)
        {
            var nlogConfig = new LoggingConfiguration();

            Target nlogTarget = CreateConsoleTarget(loggingOptions.Format);

            if (loggingOptions.Async)
            {
                nlogTarget = new AsyncTargetWrapper(TARGET_ASYNC_CONSOLE, nlogTarget);
            }

            nlogConfig.AddTarget(nlogTarget);
            nlogConfig.AddRule(LogLevel.Trace, LogLevel.Fatal, nlogTarget);
            return nlogConfig;
        }

        private static ColoredConsoleTarget CreateConsoleTarget(LogFormat format)
            => new ColoredConsoleTarget(name: TARGET_CONSOLE)
            {
                Layout = format.GetLayout()
            };
    }
}