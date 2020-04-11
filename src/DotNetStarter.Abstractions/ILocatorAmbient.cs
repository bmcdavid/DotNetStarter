using System;
using System.Collections;

namespace DotNetStarter.Abstractions
{
    /// <summary>
    /// Provides an ambient Locator where Current may be the current scoped open, or the singleton locator within the startup context
    /// </summary>
    public interface ILocatorAmbient
    {
        /// <summary>
        /// Scoped or StartupContext's locator
        /// </summary>
        ILocator Current { get; }

        /// <summary>
        /// Quick check to determine if current is in a scope
        /// </summary>
        bool IsScoped { get; }

        /// <summary>
        /// ASP.NET cannot gurantee AsyncLocal in the pipeline this is a mechanism to bypass this
        /// </summary>
        /// <param name="contextStorage"></param>
        void PreferredStorageAccessor(Func<IDictionary> contextStorage);
    }
}