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
            var layout = LogFormat.Gelf.GetLayout();

            layout.Should().BeOfType<GelfLayout>();
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