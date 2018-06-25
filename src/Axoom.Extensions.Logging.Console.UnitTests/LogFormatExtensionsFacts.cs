using System;
using Axoom.Extensions.Logging.Console.Layouts;
using FluentAssertions;
using Xunit;

namespace Axoom.Extensions.Logging.Console
{
    public class LogFormatExtensionsFacts
    {
        [Fact]
        public void GettingLayoutForGelfReturnsGelfLayout()
        {
#pragma warning disable 618
            var layout = LogFormat.Gelf.GetLayout();
#pragma warning restore 618

            layout.Should().BeOfType<GelfLayout>();
        }
        
        [Fact]
        public void GettingLayoutForJsonReturnsJsonLayout()
        {
            var layout = LogFormat.Json.GetLayout();

            layout.Should().BeOfType<JsonOutputLayout>();
        }

        [Fact]
        public void GettingLayoutForPlainReturnsPlainLayout()
        {
            var layout = LogFormat.Plain.GetLayout();

            layout.Should().BeOfType<PlainLayout>();
        }

        [Fact]
        public void GettingLayoutForUnknownThrowsArgumentOutOfRangeException()
        {
            Action gettingLayout = () => ((LogFormat)999).GetLayout();

            gettingLayout.Should().Throw<ArgumentOutOfRangeException>();
        }
    }
}