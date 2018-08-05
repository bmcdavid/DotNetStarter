using DotNetStarter.Abstractions;
using System;
using System.Collections.Generic;

namespace DotNetStarter.Configure.Expressions
{
    /// <summary>
    /// Provides access to remove ILocatorConfigure and IStartupModule types from startup process
    /// </summary>
    public sealed class StartupModulesExpression
    {
        internal readonly HashSet<Type> RemoveModuleTypes = new HashSet<Type>();
        private Action<ILocatorConfigureModuleCollection> _locatorConfigureCollection;
        private Action<IStartupModuleCollection> _startupModuleCollection;

        internal ILocatorConfigureModuleCollection InternalConfigureModules { get; private set; }
        internal IStartupModuleCollection InternalStartupModules { get; private set; }

        /// <summary>
        /// Provides fluent access to assigning an ILocatorConfigure module during a startupbuilder
        /// <para>Added ILocatorConfigure modules will execute after RegistrationConfiguration</para>
        /// </summary>
        /// <param name="locatorConfigureModuleCollection"></param>
        /// <returns></returns>
        public StartupModulesExpression ConfigureLocatorModuleCollection(Action<ILocatorConfigureModuleCollection> locatorConfigureModuleCollection)
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
        /// Removes the given type from the startup/shutdown or configuration process
        /// </summary>
        /// <param name="moduleType"></param>
        /// <returns></returns>
        public StartupModulesExpression RemoveModule(Type moduleType)
        {
            RemoveModuleTypes.Add(moduleType);
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
            InternalStartupModules = startupModuleCollection;

            var locatorModuleCollection = new LocatorConfigureCollection();
            _locatorConfigureCollection?.Invoke(locatorModuleCollection);
            InternalConfigureModules = locatorModuleCollection;
        }
    }
}