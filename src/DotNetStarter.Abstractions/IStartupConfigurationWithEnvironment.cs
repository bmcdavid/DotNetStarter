namespace DotNetStarter.Abstractions
{
    //todo: v2, add reference to IStartupEnvironment

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