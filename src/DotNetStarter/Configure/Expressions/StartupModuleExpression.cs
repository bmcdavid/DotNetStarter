using DotNetStarter.Abstractions;
using System;
using System.Collections.Generic;
using System.Linq;

namespace DotNetStarter.Configure.Expressions
{
    /// <summary>
    /// Provides access to remove ILocatorConfigure and IStartupModule types from startup process
    /// </summary>
    public sealed class StartupModulesExpression
    {
        internal readonly HashSet<Type> RemoveModuleTypes = new HashSet<Type>();
        private Action<ILocatorConfigureCollection> _locatorConfigureCollection;
        private Action<IStartupModuleCollection> _startupModuleCollection;

        /// <summary>
        /// Constructor
        /// </summary>
        public StartupModulesExpression()
        {
            //reset on every expression instance
            ManualStartupModules.InternalModules.Clear();
            ManualLocatorConfigureModules.InternalModules.Clear();
        }

        /// <summary>
        /// Provides fluent access to assigning an ILocatorConfigure module during a startupbuilder
        /// <para>Added ILocatorConfigure modules will execute after RegistrationConfiguration</para>
        /// </summary>
        /// <param name="locatorConfigureModuleCollection"></param>
        /// <returns></returns>
        public StartupModulesExpression ConfigureLocatorModuleCollection(Action<ILocatorConfigureCollection> locatorConfigureModuleCollection)
        {
            _locatorConfigureCollection += locatorConfigureModuleCollection;
            return this;
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
            ManualStartupModules.InternalModules.AddRange(startupModuleCollection);

            var locatorModuleCollection = new LocatorConfigureCollection();
            _locatorConfigureCollection?.Invoke(locatorModuleCollection);
            ManualLocatorConfigureModules.InternalModules.AddRange(locatorModuleCollection);
        }

        /// <summary>
        /// Wraps ILocatorConfigure added using the fluent configuration
        /// </summary>
        [StartupModule(typeof(RegistrationConfiguration))]
        public class ManualLocatorConfigureModules : ILocatorConfigure
        {
            internal static readonly List<ILocatorConfigure> InternalModules = new List<ILocatorConfigure>();

            /// <summary>
            /// Constructor
            /// </summary>
            public ManualLocatorConfigureModules()
            {
                Modules = new List<ILocatorConfigure>();
            }

            /// <summary>
            /// Access to module descriptors added during last startup builder
            /// </summary>
            public List<ILocatorConfigure> Modules { get; }

            void ILocatorConfigure.Configure(ILocatorRegistry registry, IStartupEngine engine)
            {
                Modules.AddRange(InternalModules);

                foreach(var configureModule in Modules)
                {
                    configureModule.Configure(registry, engine);
                }
            }
        }

        /// <summary>
        /// Wraps IStartupModule added using the fluent configuration
        /// </summary>
        [StartupModule(typeof(RegistrationConfiguration))]
        public class ManualStartupModules : IStartupModule, ILocatorConfigure
        {
            internal static readonly List<StartupModuleDescriptor> InternalModules = new List<StartupModuleDescriptor>();
            private static readonly Type StartupModuleType = typeof(IStartupModule);
            private readonly List<IStartupModule> _modules = new List<IStartupModule>();

            /// <summary>
            /// Access to module descriptors added during last configuration
            /// </summary>
            public static List<StartupModuleDescriptor> Modules => new List<StartupModuleDescriptor>(InternalModules);

            void ILocatorConfigure.Configure(ILocatorRegistry registry, IStartupEngine engine)
            {
                foreach (var module in InternalModules)
                {
                    if (module.ModuleType == null) continue;

                    //todo: determine if registered modules should be resolved for all IStartupModules
                    //registry.Add(StartupModuleType, module.ModuleType, Lifecycle.Singleton); // for resolution of All IStartupModule
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
                    InternalModules
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