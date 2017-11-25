namespace Axoom.Extensions.Logging.Console.Layouts
{
    internal static class LayoutFormats
    {
        public const string TIMESTAMP = "${date:universalTime=true:format=o}";
        public const string TIMESTAMP_UNIX = "${unixtime:universalTime=true}";
        public const string SYS_LOGLEVEL = "${sysloglevel}";
        public const string LOGLEVEL = "${level:upperCase=true}";
        public const string LOGLEVEL_PADDED = "${pad:padding=-5:fixedLength=True:" + LOGLEVEL + "}";
        public const string LOGGER = "${logger}";
        public const string EXCEPTION_TYPE = "${exception:format=type";
        public const string EXCEPTION_MESSAGE = "${exception:format=message";
        public const string EXCEPTION_STACKTRACE = "${exception:format=stacktrace";
        public const string EXCEPTION = "${exception:format=toString}";
        public const string EXCEPTION_WITH_NEWLINE = "${onexception:${newline}" + EXCEPTION;
        public const string MESSAGE = "${message}";
        public const string MESSAGE_WITH_EXCEPTION = MESSAGE + EXCEPTION_WITH_NEWLINE;

        public const string CALLSITE =
            "${callsite:className=true:FileName=false:IncludeSourcePath=false:cleanNamesOfAnonymousDelegates=true:includeNamespace=false}";
    }
}