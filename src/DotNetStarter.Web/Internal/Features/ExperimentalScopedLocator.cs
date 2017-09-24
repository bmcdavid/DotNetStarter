using DotNetStarter.Abstractions;
using System;

namespace DotNetStarter.Web.Internal.Features
{
    /// <summary>
    /// Exprimental feature: Enables new scoping mechanism
    /// </summary>
    [Register(typeof(IStartupFeature), LifeTime.Singleton)]
    [Register(typeof(ExperimentalScopedLocator), LifeTime.Singleton)]
    public class ExperimentalScopedLocator : IStartupFeature
    {
        static Type _Type = typeof(ExperimentalScopedLocator);

        /// <summary>
        /// Default for netframework, set app setting for 'DotNetStarter.Web.Internal.Features.ExperimentalScopedLocator' = true
        /// </summary>
        public virtual bool Enabled
        {
            get
            {
#if NETSTANDARD1_3
                return false;
#else
                return System.Configuration.ConfigurationManager.AppSettings[Name] == "true";
#endif
            }
        }

        /// <summary>
        /// Feature name
        /// </summary>
        public string Name => _Type.FullName;
    }
}
