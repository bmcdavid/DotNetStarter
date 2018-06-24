using System.Collections.Generic;

namespace DotNetStarter.Abstractions
{
    /// <summary>
    /// A collection of StartupModuleDescriptor to run during startup
    /// </summary>
    public interface IStartupModuleCollection : IList<StartupModuleDescriptor>
    {
    }

    /// <summary>
    /// Default IStartupModuleCollection
    /// </summary>
    public class StartupModuleCollection : List<StartupModuleDescriptor>, IStartupModuleCollection
    {
    }

    /// <summary>
    /// Extensions for IStartupCollection
    /// </summary>
    public static class StartupModuleExtensions
    {
        /// <summary>
        /// Adds instance to given IStartupCollection
        /// </summary>
        /// <param name="collection"></param>
        /// <param name="instance"></param>
        /// <returns></returns>
        public static IStartupModuleCollection AddInstance(this IStartupModuleCollection collection,
            IStartupModule instance)
        {
            collection.Add(new StartupModuleDescriptor { ModuleInstance = instance });
            return collection;
        }

        /// <summary>
        /// Adds type T to give IStartupModuleCollection
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="collection"></param>
        /// <returns></returns>
        public static IStartupModuleCollection AddType<T>(this IStartupModuleCollection collection) where T : IStartupModule
        {
            collection.Add(new StartupModuleDescriptor().UseModuleType<T>());
            return collection;
        }
    }
}