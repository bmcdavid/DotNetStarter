using System;

namespace DotNetStarter.Abstractions
{
    /// <summary>
    /// Used by fluent configuration
    /// </summary>
    public class StartupModuleDescriptor
    {
        private Type _moduleType;

        /// <summary>
        /// IStartupModule instance
        /// </summary>
        public IStartupModule ModuleInstance { get; set; }

        /// <summary>
        /// Access to Type for ILocator to resolve. If only providing type, please use UseModuleType!
        /// </summary>
        public Type ModuleType => ModuleInstance?.GetType() ?? _moduleType;

        /// <summary>
        /// IStartupModule type, when used is resolved from ILocator
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public StartupModuleDescriptor UseModuleType<T>() where T : IStartupModule
        {
            _moduleType = typeof(T);
            return this;
        }
    }
}