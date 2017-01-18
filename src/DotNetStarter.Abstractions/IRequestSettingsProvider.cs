namespace DotNetStarter.Abstractions
{
    using System.Collections;

    /// <summary>
    /// Abstraction for HttpContext.Item and HttpContext.IsDebugMode
    /// </summary>
    public interface IRequestSettingsProvider
    {
        /// <summary>
        /// Item dictionary of request
        /// </summary>
        IDictionary Items { get; }

        /// <summary>
        /// Debug mode
        /// </summary>
        bool IsDebugMode { get; }
    }
}