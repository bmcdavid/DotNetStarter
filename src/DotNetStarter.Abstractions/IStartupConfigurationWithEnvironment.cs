namespace DotNetStarter.Abstractions
{

    /// <summary>
    /// enhanced startup configuration with environment reference
    /// </summary>
    public interface IStartupConfigurationWithEnvironment<TEnvironment> : IStartupConfigurationWithEnvironment where TEnvironment : IStartupEnvironment
    {
        /// <summary>
        /// Startup environment reference
        /// </summary>
        new TEnvironment Environment { get; }
    }

    /// <summary>
    /// enhanced startup configuration with environment reference
    /// </summary>
    public interface IStartupConfigurationWithEnvironment : IStartupConfiguration
    {
        /// <summary>
        /// Startup environment reference
        /// </summary>
        IStartupEnvironment Environment { get; }
    }
}