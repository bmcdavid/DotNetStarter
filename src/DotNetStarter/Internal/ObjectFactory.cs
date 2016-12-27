namespace DotNetStarter
{
    using Abstractions;
    using Internal;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Reflection;

    internal static class ObjectFactory
    {
        private static readonly object _Lock = new object();

        private static IStartupObjectFactory _Default = null;

        internal static IEnumerable<Assembly> Assemblies;

        static ObjectFactory()
        {
            if (_Default == null)
            {
                lock (_Lock)
                {
                    if (_Default == null)
                    {
                        Assemblies = AssemblyLoader.Default.GetAssemblies();
                        var configurations = Assemblies.SelectMany(x => x.CustomAttribute(typeof(StartupObjectFactoryAttribute), false)).OfType<StartupObjectFactoryAttribute>();
                        var configurationInstances = configurations.Select(x => Activator.CreateInstance(x.FactoryType)).OfType<IStartupObjectFactory>();
                        var sortedIntances = configurationInstances.OrderBy(x => x.SortOrder);

                        _Default = sortedIntances.LastOrDefault();
                    }
                }
            }
        }

        public static IStartupObjectFactory Default => _Default;
    }
}