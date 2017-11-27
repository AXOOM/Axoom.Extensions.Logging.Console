using System;
using Axoom.Extensions.Logging.Console.Layouts;
using FluentAssertions;
using NLog.Layouts;
using Xunit;

namespace Axoom.Extensions.Logging.Console
{
    public class LogFormatExtensionsFacts
    {
        [Fact]
        public void GettingLayoutForGelfReturnsGelfLayout()
        {
            Layout layout = LogFormat.Gelf.GetLayout();

            layout.Should().BeOfType<GelfLayout>();
        }
        
        [Fact]
        public void GettingLayoutForPlainReturnsPlainLayout()
        {
            Layout layout = LogFormat.Plain.GetLayout();

            layout.Should().BeOfType<PlainLayout>();
        }
        
        [Fact]
        public void GettingLayoutForUnknownThrowsArgumentOutOfRangeException()
        {
            Action gettingLayout = () => ((LogFormat)999).GetLayout();

            gettingLayout.ShouldThrow<ArgumentOutOfRangeException>();
        }
    }
}