using DotNetStarter.Abstractions;
using System;
using System.Collections.Generic;
using System.Linq;

namespace DotNetStarter.Configure
{
    /// <summary>
    /// Provides access to remove ILocatorConfigure and IStartupModule types from startup process
    /// </summary>
    public sealed class StartupModulesExpression
    {
        internal readonly HashSet<Type> RemoveModuleTypes = new HashSet<Type>();
        private Action<IStartupModuleCollection> _startupModuleCollection;

        /// <summary>
        /// Constructor
        /// </summary>
        public StartupModulesExpression()
        {
            ManualStartupModules.Modules.Clear(); //reset foreach expression instance
        }

        /// <summary>
        /// Provides fluent access to assigning a module type or instance
        /// </summary>
        /// <param name="startupModuleCollection"></param>
        /// <returns></returns>
        public StartupModulesExpression ConfigureStartupModuleCollection(Action<IStartupModuleCollection> startupModuleCollection)
        {
            _startupModuleCollection += startupModuleCollection;
            return this;
        }

        /// <summary>
        /// Removes the given type from the configuration of the ILocator
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public StartupModulesExpression RemoveConfigureModule<T>() where T : ILocatorConfigure
        {
            RemoveModuleTypes.Add(typeof(T));
            return this;
        }

        /// <summary>
        /// Removes the given type from the startup/shutdown process
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public StartupModulesExpression RemoveStartupModule<T>() where T : IStartupModule
        {
            RemoveModuleTypes.Add(typeof(T));
            return this;
        }

        /// <summary>
        /// Configures the IStartupModule collection for ManualStartupModule
        /// </summary>
        internal void Build()
        {
            var startupModuleCollection = new StartupModuleCollection();
            _startupModuleCollection?.Invoke(startupModuleCollection);
            ManualStartupModules.Modules.AddRange(startupModuleCollection);
        }

        /// <summary>
        /// Wraps IStartupModule added using the fluent configuration
        /// </summary>
        [StartupModule(typeof(RegistrationConfiguration))]
        public class ManualStartupModules : IStartupModule, ILocatorConfigure
        {
            internal static readonly List<StartupModuleDescriptor> Modules = new List<StartupModuleDescriptor>();
            private static readonly Type StartupModuleType = typeof(IStartupModule);
            private readonly List<IStartupModule> _modules = new List<IStartupModule>();

            /// <summary>
            /// Access to module descriptors added during last configuration
            /// </summary>
            public static List<StartupModuleDescriptor> ReadonlyModules => new List<StartupModuleDescriptor>(Modules);

            void ILocatorConfigure.Configure(ILocatorRegistry registry, IStartupEngine engine)
            {
                foreach (var module in Modules)
                {
                    if (module.ModuleType == null) continue;

                    //todo: determine if registered modules should be resolved for all IStartupModules
                    //registry.Add(StartupModuleType, module.ModuleType); // for resolution of All modules
                    registry.Add(module.ModuleType, module.ModuleType, lifecycle: Lifecycle.Singleton);
                }
            }

            void IStartupModule.Shutdown()
            {
                foreach (var module in _modules)
                {
                    module.Shutdown();
                }
            }

            void IStartupModule.Startup(IStartupEngine engine)
            {
                // convert to modules
                _modules.AddRange
                (
                    Modules
                    .Select(x => x.ModuleInstance ?? (x.ModuleType != null ? engine.Locator.Get(x.ModuleType) as IStartupModule : null))
                    .Where(x => x != null)
                );

                foreach (var module in _modules)
                {
                    module.Startup(engine);
                }
            }
        }
    }
}