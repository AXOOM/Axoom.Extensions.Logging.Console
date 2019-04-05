using System;
using Axoom.Extensions.Logging.Console.Layouts;
using NLog.Layouts;

namespace Axoom.Extensions.Logging.Console
{
    internal static class LogFormatExtensions
    {
        private static readonly Lazy<Layout> _plainLayout = new Lazy<Layout>(() => new PlainLayout());
        private static readonly Lazy<JsonOutputLayout> _jsonLayout = new Lazy<JsonOutputLayout>(() => new JsonOutputLayout());

        public static Layout GetLayout(this LogFormat format)
        {
            switch (format)
            {
                case LogFormat.Json:
                    return _jsonLayout.Value;
                case LogFormat.Plain:
                    return _plainLayout.Value;
                default:
                    throw new ArgumentOutOfRangeException(nameof(format));
            }
        }
    }
}
