using DotNetStarter.Abstractions;
using DotNetStarter.Configure.Expressions;
using System;
using System.Collections.Generic;
using System.Reflection;

namespace DotNetStarter.Configure
{
    /// <summary>
    /// Provides fluent api for DotNetStarter Configuration
    /// <para>IMPORTANT: For netstandard 1.0 applications, ConfigureAssemblies MUST be used as there is no default assembly loader!</para>
    /// </summary>
    public sealed class StartupBuilder
    {
        private static readonly object _objLock = new object();
        private static bool _appStarting = false;
        private Action<AssemblyExpression> _assemblyExpression;
        private IStartupEnvironment _environment;
        private bool _isConfigured;
        private Action<StartupModulesExpression> _moduleExpression;
        private Action<OverrideExpression> _overrideExpression;
        private bool _runOnce;
        private IStartupHandler _startupHandler;
        private StartupBuilder() { }

        /// <summary>
        /// The IStartupContext result after Run has been called
        /// </summary>
        public IStartupContext StartupContext { get; private set; }
        /// <summary>
        /// Creates a new instance of a StartupBuilder object
        /// </summary>
        /// <returns></returns>
        public static StartupBuilder Create() { return new StartupBuilder(); }

        /// <summary>
        /// Runs expressions, and configures DotNetStarter's ILocator, but does not run IStartupModules
        /// <para>IMPORTANT: Must run after all other configurations.</para>
        /// </summary>
        /// <param name="useApplicationContext">If false, the static ApplicationContext.Default will not be set after execution. Default is true.</param>
        /// <returns></returns>
        public StartupBuilder Build(bool useApplicationContext = true)
        {
            if (_isConfigured) { return this; }

            _isConfigured = true;
            var objFactory = new StartupBuilderObjectFactory() { Environment = _environment };
            var assemblyExp = new AssemblyExpression();
            _assemblyExpression?.Invoke(assemblyExp);
            objFactory.AssemblyExpression = assemblyExp;

            var moduleExp = new StartupModulesExpression();
            _moduleExpression?.Invoke(moduleExp);
            moduleExp.Build();
            objFactory.StartupModulesExpression = moduleExp;

            var overrideExp = new OverrideExpression();
            _overrideExpression?.Invoke(overrideExp);
            objFactory.OverrideExpression = overrideExp;

            //todo: need to store IStartupHandler to invoke delayed startup.

            // default way using the static startup
            if (useApplicationContext)
            {
                // if no assemblies have been configured follow the default scanner rule
                var assemblies = assemblyExp.Assemblies.Count > 0 ? assemblyExp.Assemblies : null;
                EnsureStartup(a => objFactory.CreateStartupConfiguration(a,objFactory.Environment), null, assemblies: assemblies);
                StartupContext = ApplicationContext.Default;
                return this;
            }

            if (assemblyExp.Assemblies.Count == 0)
            {
                throw new Internal.AssembliesNotConfiguredException();
            }

            // allows for non static execution
            var startupConfig = objFactory.CreateStartupConfiguration(assemblyExp.Assemblies, startupEnvironment: null);
            // change StartupHandler in override to Func<IStartupConfig,IStartupHandler>
            _startupHandler = overrideExp.StartupHandler ??
                new StartupHandler(objFactory.CreateTimedTask, objFactory.CreateRegistry(startupConfig), objFactory.CreateContainerDefaults());

            StartupContext = _startupHandler.ConfigureLocator(startupConfig);
            return this;
        }

        /// <summary>
        /// Configures assemblies for DotNetStarter to scan for IStartup modules, ILocatorConfigure modules, and types with RegistrationAttribute
        /// </summary>
        /// <param name="assemblyExpression"></param>
        /// <returns></returns>
        public StartupBuilder ConfigureAssemblies(Action<AssemblyExpression> assemblyExpression)
        {
            _assemblyExpression += assemblyExpression;
            return this;
        }
        /// <summary>
        /// Customize IStartupModule and ILocatorConfigure types in the startup process
        /// </summary>
        /// <param name="moduleExpression"></param>
        /// <returns></returns>
        public StartupBuilder ConfigureStartupModules(Action<StartupModulesExpression> moduleExpression)
        {
            _moduleExpression += moduleExpression;
            return this;
        }

        /// <summary>
        /// Overrides default services
        /// </summary>
        /// <param name="overrideExpression"></param>
        /// <returns></returns>
        public StartupBuilder OverrideDefaults(Action<OverrideExpression> overrideExpression)
        {
            _overrideExpression = overrideExpression;
            return this;
        }

        /// <summary>
        /// Runs IStartupModules
        /// </summary>
        public void Run()
        {
            if (_runOnce) return;

            _runOnce = true;
            Build(); // just in case its not called fluently
            _startupHandler.StartupModules();
        }

        /// <summary>
        /// Provides a custom startup environment instance for the startup process
        /// </summary>
        /// <param name="startupEnvironment"></param>
        /// <returns></returns>
        public StartupBuilder UseEnvironment(IStartupEnvironment startupEnvironment)
        {
            _environment = startupEnvironment;
            return this;
        }

        internal void EnsureStartup(Func<IEnumerable<Assembly>, IStartupConfiguration> configurationFactory, Func<IStartupConfiguration, IStartupHandler> handlerFactory, IEnumerable<Assembly> assemblies = null)
        {
            if (!ApplicationContext.Started)
            {
                if (_appStarting)
                {
                    throw new Exception($"Do not access {typeof(ApplicationContext).FullName}.{nameof(ApplicationContext.Default)} during startup!");
                }

                lock (_objLock)
                {
                    if (!ApplicationContext.Started)
                    {
                        _appStarting = true;
                        var assembliesForStartup = assemblies ?? new Internal.AssemblyLoader().GetAssemblies();
                        var startupConfig = configurationFactory.Invoke(assemblies);
                        var handler = handlerFactory.Invoke(startupConfig);

                        //new StartupHandler(factory.CreateTimedTask, factory.CreateRegistry(startupConfig), factory.CreateContainerDefaults());

                        ApplicationContext._Default = handler.ConfigureLocator(startupConfig);
                        ApplicationContext.Started = ApplicationContext._Default != null;
                        _appStarting = false;
                    }
                }
            }
        }
    }
}