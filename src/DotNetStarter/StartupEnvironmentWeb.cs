using DotNetStarter.Abstractions;

namespace DotNetStarter
{
    /// <summary>
    /// Startup Environment for web
    /// </summary>
    public class StartupEnvironmentWeb : StartupEnvironment, IStartupEnvironmentWeb
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="environmentName">Required, for netcore applications, assign values from Microsoft.AspNetCore.Hosting.IHostingEnvironment, for netframework pass a value from appsettings such as'DotNetStarter.Environment'.</param>
        /// <param name="applicationBasePath"></param>
        /// <param name="webRootPath"></param>
        /// <param name="contentRootPath"></param>
        public StartupEnvironmentWeb(string environmentName, string applicationBasePath = null, string webRootPath = null, string contentRootPath = null) :
            base
            (
                environmentName,
                applicationBasePath ?? GetBaseDirectory()
            )
        {
            WebRootPath = webRootPath ?? GetWebRootPath();
            ContentRootPath = contentRootPath ?? WebRootPath;
        }

        /// <summary>
        /// Full webroot path of environment
        /// </summary>
        public virtual string WebRootPath { get; }

        /// <summary>
        /// Full content root path of environment
        /// </summary>
        public virtual string ContentRootPath { get; }

        private static string GetBaseDirectory()
        {
#if NETSTANDARD1_0 || NETSTANDARD1_6
            return null;
#else
            return System.AppDomain.CurrentDomain.BaseDirectory;
#endif
        }

        private static string GetWebRootPath()
        {
#if NETSTANDARD
            return null;
#else
            var appDomain = System.AppDomain.CurrentDomain;
            return !string.IsNullOrEmpty(appDomain.SetupInformation.PrivateBinPath) ? appDomain.BaseDirectory : null;
#endif
        }
    }
}