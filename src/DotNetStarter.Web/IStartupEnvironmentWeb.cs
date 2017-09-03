using DotNetStarter.Abstractions;

namespace DotNetStarter.Web
{
    /// <summary>
    /// Information about current web environment
    /// </summary>
    public interface IStartupEnvironmentWeb : IStartupEnvironment
    {
        /// <summary>
        /// Full webroot path of environment
        /// </summary>
        string WebRootPath { get; }

        /// <summary>
        /// Full content root path of environment
        /// </summary>
        string ContentRootPath { get; }
    }
}
