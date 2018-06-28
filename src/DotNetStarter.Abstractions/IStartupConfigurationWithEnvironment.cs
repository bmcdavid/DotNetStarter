using System;

namespace DotNetStarter.Abstractions
{

    /// <summary>
    /// enhanced startup configuration with environment reference
    /// </summary>
    [Obsolete]
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
    [Obsolete]
    public interface IStartupConfigurationWithEnvironment : IStartupConfiguration
    {

    }
}