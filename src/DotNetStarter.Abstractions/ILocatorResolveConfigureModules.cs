using System.Collections.Generic;

namespace DotNetStarter.Abstractions
{
    public interface ILocatorResolveConfigureModules
    {
        IEnumerable<ILocatorConfigure> ResolveConfigureModules(IEnumerable<IDependencyNode> filteredModules);
    }
}
