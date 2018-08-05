namespace DotNetStarter.Abstractions
{
    /// <summary>
    /// Init module that configures the container object.
    /// <para>IMPORTANT: For implementations using assembly scanning for discovery, those types require an empty constructor and require [StartupModuleAttribute]; StartupBuilder has options to pass manually created instances!</para>
    /// </summary>
    [CriticalComponent]
    public interface ILocatorConfigure
    {
        /// <summary>s
        /// Configure container object
        /// </summary>
        /// <param name="registry">Container instance</param>
        /// <param name="engine">Configuration and events to subscribe to.</param>
        void Configure(ILocatorRegistry registry, ILocatorConfigureEngine engine);
    }
}