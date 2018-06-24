using DotNetStarter.Abstractions;
using System;
using System.Collections.Generic;
using System.Linq;

namespace DotNetStarter.Configure
{
    internal class StartupBuilderModuleFilter : IStartupModuleFilter
    {
        private readonly ICollection<Type> _moduleTypesToRemove;

        public StartupBuilderModuleFilter(ICollection<Type> moduleTypesToRemove)
        {
            _moduleTypesToRemove = moduleTypesToRemove;
        }

        public IEnumerable<IDependencyNode> FilterModules(IEnumerable<IDependencyNode> modules)
        {
            return modules.Where
            (
                m =>
                m.Node is Type nodeType &&
                !_moduleTypesToRemove.Contains(nodeType)
            );
        }
    }
}