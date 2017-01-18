namespace DotNetStarter.Abstractions
{
    using System;

    /// <summary>
    /// Provides access to create locator registry object
    /// </summary>
    public class LocatorRegistryFactoryAttribute : AssemblyFactoryBaseAttribute
    {
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="locatorRegistryFactory"></param>
        /// <param name="dependencies"></param>
        public LocatorRegistryFactoryAttribute(Type locatorRegistryFactory, params Type[] dependencies) :
            base(locatorRegistryFactory, typeof(ILocatorRegistryFactory), dependencies)
        { }
    }
}
