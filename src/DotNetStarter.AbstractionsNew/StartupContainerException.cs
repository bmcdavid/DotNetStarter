namespace DotNetStarter.Abstractions
{
    using System;

    /// <summary>
    /// Exceptions thrown by IContainer(s)
    /// </summary>
    public class StartupContainerException : Exception
    {
        /// <summary>
        /// Error code
        /// </summary>
        public virtual int ErrorCode { get; }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="code"></param>
        /// <param name="message"></param>
        /// <param name="inner"></param>
        public StartupContainerException(int code, string message, Exception inner) : base(message, inner)
        {
            ErrorCode = code;
        }
    }
}