namespace DotNetStarter.Abstractions
{
    using System;

    /// <summary>
    /// Adds contains service ability to ILocatorRegistry implementations
    /// </summary>
    public interface ILocatorRegistryWithContains
    {
        /// <summary>
        /// Determines if container has service
        /// </summary>
        /// <param name="serviceType"></param>
        /// <param name="key"></param>
        /// <returns></returns>
        bool ContainsService(Type serviceType, string key = null);
    }
}