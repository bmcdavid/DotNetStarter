namespace DotNetStarter.Abstractions
{
    /// <summary>
    /// Initalization log levels
    /// </summary>
    public enum LogLevel
    {
        /// <summary>Trace log level.</summary>
        Trace = 0,

        /// <summary>Debug log level.</summary>
        Debug = 100,

        /// <summary>Info log level.</summary>
        Info = 200,

        /// <summary>Warn log level.</summary>
        Warn = 300,

        /// <summary>Error log level.</summary>
        Error = 400,

        /// <summary>Fatal log level.</summary>
        Fatal = 500,
    }
}