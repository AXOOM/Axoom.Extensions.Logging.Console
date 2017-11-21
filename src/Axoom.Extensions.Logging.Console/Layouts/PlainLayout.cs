using NLog.Layouts;

namespace Axoom.Extensions.Logging.Console.Layouts
{
    public class PlainLayout : SimpleLayout
    {
        public PlainLayout()
        {
            Text = $"{LayoutFormats.TIMESTAMP} | " +
                   $"{LayoutFormats.LOGLEVEL_PADDED} | " +
                   $"{LayoutFormats.LOGGER} | " +
                   $"{LayoutFormats.CALLSITE} | " +
                   $"{LayoutFormats.MESSAGE}" +
                   LayoutFormats.EXCEPTION_WITH_NEWLINE;
        }
    }
}