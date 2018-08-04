using System;

namespace DotNetStarter.Abstractions
{
    /// <summary>
    /// Used by fluent configuration
    /// </summary>
    public class StartupModuleDescriptor
    {
        /// <summary>
        /// IStartupModule instance
        /// </summary>
        public IStartupModule ModuleInstance { get; set; }

        /// <summary>
        /// Access to Type for ILocator to resolve. If only providing type, please use UseModuleType!
        /// </summary>
        public Type ModuleType { get; private set; }

        /// <summary>
        /// IStartupModule type, when used is resolved from ILocator
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public StartupModuleDescriptor UseModuleType<T>() where T : IStartupModule
        {
            ModuleType = typeof(T);
            return this;
        }
    }
}