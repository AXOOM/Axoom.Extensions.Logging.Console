using NLog.Layouts;

namespace Axoom.Extensions.Logging.Console.Layouts
{
    public class PlainLayout : SimpleLayout
    {
        public PlainLayout()
        {
            Text = $"{LayoutFormats.TIMESTAMP_ISO8601} | " +
                   $"{LayoutFormats.LOGLEVEL_PADDED} | " +
                   $"{LayoutFormats.LOGGER} | " +
                   $"{LayoutFormats.CALLSITE} | " +
                   $"{LayoutFormats.MESSAGE}" +
                   LayoutFormats.EXCEPTION_WITH_NEWLINE;
        }
    }
}