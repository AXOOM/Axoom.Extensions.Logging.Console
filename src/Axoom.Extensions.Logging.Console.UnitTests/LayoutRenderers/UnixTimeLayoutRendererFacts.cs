using System;
using FluentAssertions;
using NLog;
using Xunit;

namespace Axoom.Extensions.Logging.Console.LayoutRenderers
{
    public class UnixTimeLayoutRendererFacts
    {
        private readonly UnixTimeLayoutRenderer _layoutRenderer;

        public UnixTimeLayoutRendererFacts()
        {
            _layoutRenderer = new UnixTimeLayoutRenderer();
        }

        [Fact]
        public void RendersCorrectToUnixTime()
        {
            var logEventInfo = new LogEventInfo(LogLevel.Info, "ALogger", "A message")
            {
                TimeStamp = new DateTime(2017, 08, 30, 11, 24, 15, 345, DateTimeKind.Utc)
            };

            string output = _layoutRenderer.Render(logEventInfo);
            
            output.ShouldBeEquivalentTo(1504092255);
        }
    }
}