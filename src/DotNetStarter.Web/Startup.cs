﻿#if !NETSTANDARD1_3

using DotNetStarter.Abstractions;
using System;

namespace DotNetStarter.Web
{
    /// <summary>
    /// Registers the HttpModule for starting up IHttpModules
    /// </summary>
    [StartupModule]
    public class Startup : IStartupModule
    {
        /// <summary>
        /// If set to true (default is false), then a 'PreApplicationStartMethodAttribute' assembly attribute (https://msdn.microsoft.com/en-us/library/system.web.preapplicationstartmethodattribute.aspx)
        /// must be used to execute DotNetStarter.Web.Startup.RegisterWebModuleStartup
        /// </summary>
        [Obsolete("No longed required, will be removed in version 2.")]
        public static bool DisableStartupModuleRegistration { get; set; }

        private static bool IsRegistered = false;

        void IStartupModule.Shutdown(IStartupEngine engine) { }

        void IStartupModule.Startup(IStartupEngine engine)
        {
            RegisterWebModuleStartup();
        }

        /// <summary>
        /// Registers the main DotNetStarter WebModuleStartup for netframework
        /// </summary>
        public static void RegisterWebModuleStartup()
        {
            if (IsRegistered)
            {
                return;
            }

            try
            {
                var moduleType = typeof(WebModuleStartup);
#if NET40
                // requires Microsoft.Web.Infrastructure package for .net 4.0
                Microsoft.Web.Infrastructure.DynamicModuleHelper.DynamicModuleUtility.RegisterModule(moduleType);
#elif NET45
                System.Web.HttpApplication.RegisterModule(moduleType);
#endif
            }
            catch (System.InvalidOperationException)
            {
#if NET40 || NET45
                throw new System.InvalidOperationException($"Please execute {typeof(ApplicationContext).FullName}.{nameof(ApplicationContext.Startup)} in a {typeof(System.Web.PreApplicationStartMethodAttribute)} startup method or the global asax constructor!");
#endif
            }

            IsRegistered = true;
        }
    }
}
#endif