namespace DotNetStarter.Abstractions
{
    //todo: consider making a new type for ILocatorConfigure that doesn't provide the locator, will be major volatile change though

    /// <summary>
    /// Init module that configures the container object.
    /// <para>IMPORTANT: For implementations using assembly scanning for discovery, those types require an empty constructor and require [StartupModuleAttribute]; StartupBuilder has options to pass manually created instances!</para>
    /// <para>IMPORTANT: Do not access IStartupEngine.Locator during Configure!</para>
    /// </summary>
    public interface ILocatorConfigure
    {
        /// <summary>
        /// Configure container object
        /// <para>IMPORTANT: Do not access IStartupEngine.Locator during Configure!</para>
        /// </summary>
        /// <param name="registry">Container instance</param>
        /// <param name="engine">Events to subscribe too.</param>
        void Configure(ILocatorRegistry registry, IStartupEngine engine);
    }
}