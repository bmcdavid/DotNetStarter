namespace DotNetStarter.Abstractions
{
    /// <summary>
    /// Init module that configures the container object.
    /// <para>IMPORTANT: All implementations require an empty constructor!</para>
    /// </summary>
    public interface ILocatorConfigure
    {
        /// <summary>
        /// Configure container object
        /// </summary>
        /// <param name="container">Container instance</param>
        /// <param name="engine">Events to subscribe too.</param>
        void Configure(ILocatorRegistry container, IStartupEngine engine);
    }
}