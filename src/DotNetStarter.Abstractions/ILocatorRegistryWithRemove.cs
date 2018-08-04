namespace DotNetStarter.Abstractions
{
    using System;

    /// <summary>
    /// Adds remove ability to ILocatorRegistry implementations
    /// </summary>
    public interface ILocatorRegistryWithRemove
    {
        /// <summary>
        /// Removes service from container, if serviceImplementation is null will be removed
        /// </summary>
        /// <param name="serviceType"></param>
        /// <param name="key"></param>
        /// <param name="serviceImplementation"></param>
        void Remove(Type serviceType, string key = null, Type serviceImplementation = null);
    }
}