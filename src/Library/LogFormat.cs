using System;

namespace Axoom.Extensions.Logging.Console
{
    public enum LogFormat
    {
        Plain,
        Json,
        [Obsolete("Use JsonOutputLayout instead")]
        Gelf
    }
}