using DotNetStarter.Abstractions;
using DotNetStarter.Configure.Expressions;
using System;

namespace DotNetStarter.Configure
{
    /// <summary>
    /// Provides fluent api for DotNetStarter Configuration
    /// <para>IMPORTANT: For ASP.Net Core applications, ConfigureAssemblies MUST be used as there is no default assembly loader!</para>
    /// </summary>
    public sealed class StartupBuilder
    {
        private readonly StartupBuilderObjectFactory _fluentObjectFactory;
        private Action<AssemblyExpression> _assemblyExpression;
        private bool _isConfigured;
        private Action<StartupModulesExpression> _moduleExpression;
        private Action<OverrideExpression> _overrideExpression;
        private bool _runOnce;

        /// <summary>
        /// The IStartupContext result after Run has been called
        /// </summary>
        public IStartupContext StartupContext { get; private set; }

        private StartupBuilder()
        {
            _fluentObjectFactory = new StartupBuilderObjectFactory();
        }

        /// <summary>
        /// Creates a new instance of a StartupBuilder object
        /// </summary>
        /// <returns></returns>
        public static StartupBuilder Create()
        {
            return new StartupBuilder();
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
        /// Runs expressions, and configures DotNetStarter's ILocator, but does not run IStartupModules
        /// <para>IMPORTANT: Must run after all other configurations.</para>
        /// </summary>
        /// <param name="useApplicationContext">If false, the static ApplicationContext.Default will not be set after execution. Default is true.</param>
        /// <returns></returns>
        public StartupBuilder Build(bool useApplicationContext = true)
        {
            if (_isConfigured) return this;

            _isConfigured = true;
            var assemblyExp = new AssemblyExpression();
            _assemblyExpression?.Invoke(assemblyExp);
            _fluentObjectFactory.AssemblyExpression = assemblyExp;

            var moduleExp = new StartupModulesExpression();
            _moduleExpression?.Invoke(moduleExp);
            moduleExp.Build();
            _fluentObjectFactory.StartupModulesExpression = moduleExp;

            var overrideExp = new OverrideExpression();
            _overrideExpression?.Invoke(overrideExp);
            _fluentObjectFactory.OverrideExpression = overrideExp;

            // default way using the static startup
            if (useApplicationContext)
            {
                // if no assemblies have been configured follow the default scanner rule
                var assemblies = assemblyExp.Assemblies.Count > 0 ? assemblyExp.Assemblies : null;

                // one day, perhaps this static startup goes away :)
                ApplicationContext.Startup(objectFactory: _fluentObjectFactory, assemblies: assemblies);
                StartupContext = ApplicationContext.Default;
                return this;
            }

            if (assemblyExp.Assemblies.Count == 0)
            {
                throw new Exception($"No assemblies were configured for scanning! Please add them using the {nameof(ConfigureAssemblies)} callback");
            }

            // allows for non static execution
            var startupConfig = _fluentObjectFactory.CreateStartupConfiguration(assemblyExp.Assemblies, startupEnvironment: null);
            StartupContext = ApplicationContext.RunStartup(_fluentObjectFactory, startupConfig);
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
            var configuration = StartupContext.Configuration;
            if (!(configuration is StartupBuilderConfiguration delayed))
            {
                throw new Exception($"{configuration.GetType().FullName} does not implement {typeof(StartupBuilderConfiguration).FullName}!");
            }

            delayed.DelayedStartup();
        }

        /// <summary>
        /// Provides a custom startup environment instance for the startup process
        /// </summary>
        /// <param name="startupEnvironment"></param>
        /// <returns></returns>
        public StartupBuilder UseEnvironment(IStartupEnvironment startupEnvironment)
        {
            _fluentObjectFactory.Environment = startupEnvironment;
            return this;
        }
    }
}