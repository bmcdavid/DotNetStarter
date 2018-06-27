using System.Collections.Generic;

namespace DotNetStarter.Abstractions
{
    /// <summary>
    /// A collection of ILocatorConfigure instances to run after RegistrationConfiguration
    /// </summary>
    public interface ILocatorConfigureModuleCollection : IList<ILocatorConfigure> { }

    /// <summary>
    /// Default ILocatorConfigureCollection implementation
    /// </summary>
    public class LocatorConfigureCollection : List<ILocatorConfigure>, ILocatorConfigureModuleCollection { }
}