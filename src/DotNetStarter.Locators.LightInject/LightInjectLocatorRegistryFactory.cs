using DotNetStarter.Abstractions;

namespace DotNetStarter.Locators
{
    /// <summary>
    /// Creates LightInject ILocator
    /// </summary>
    public sealed class LightInjectLocatorRegistryFactory : ILocatorRegistryFactory
    {
        /// <summary>
        /// Creates default LightInject ILocator
        /// </summary>
        /// <returns></returns>
        public ILocatorRegistry CreateRegistry()
        {
            return new LightInjectLocator();
        }
    }
}
