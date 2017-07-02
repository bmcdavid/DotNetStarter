[assembly: DotNetStarter.Abstractions.LocatorRegistryFactory(typeof(DotNetStarter.SimpleInjectorLocatorFactory))]

namespace DotNetStarter
{
    using DotNetStarter.Abstractions;

    /// <summary>
    /// Locator with SimpleInjector Container
    /// </summary>
    public class SimpleInjectorLocatorFactory : ILocatorRegistryFactory
    {
        /// <summary>
        /// Creates SimpleInjector Locator
        /// </summary>
        /// <returns></returns>
        public ILocatorRegistry CreateRegistry() => new SimpleInjectorLocator();
    }
}