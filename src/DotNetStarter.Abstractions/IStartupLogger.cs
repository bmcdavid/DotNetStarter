namespace DotNetStarter.Abstractions
{
    using System;

    /// <summary>
    /// startup logger
    /// </summary>
    public interface IStartupLogger
    {
        /// <summary>
        /// Log intialization exception
        /// </summary>
        /// <param name="message"></param>
        /// <param name="e"></param>
        /// <param name="source"></param>
        /// <param name="level"></param>
        void LogException(string message, Exception e, Type source, LogLevel level);
    }
}