using DotNetStarter.Abstractions;
using DotNetStarter.Configure.Expressions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace DotNetStarter.Configure
{
    /// <summary>
    /// Provides fluent api for DotNetStarter Configuration
    /// <para>IMPORTANT: For netstandard 1.0 applications, ConfigureAssemblies MUST be used as there is no default assembly loader!</para>
    /// </summary>
    public sealed class StartupBuilder
    {
        private Action<AssemblyExpression> _assemblyExpression;
        private IStartupEnvironment _environment;
        private bool _isConfigured;
        private Action<StartupModulesExpression> _moduleExpression;
        private Action<DefaultsExpression> _overrideExpression;
        private Action<IRegistrationCollection> _registrationCollectionExpression;
        private bool _runOnce;
        private IStartupHandler _startupHandler;
        private bool _usingImport = true;

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
        /// <param name="useDiscoverableAssemblies">Ignored if assemblyexpression is used! Instructs default assembly loader to filter for assemblies with DiscoverableAssemblyAttribute</param>
        /// <returns></returns>
        public StartupBuilder Build(bool useDiscoverableAssemblies = false)
        {
            if (_isConfigured) { return this; }
            _isConfigured = true;

            AddManualRegistrations();

            var objFactory = new StartupBuilderObjectFactory() { Environment = _environment };
            var assemblyExp = new AssemblyExpression();
            _assemblyExpression?.Invoke(assemblyExp);
            objFactory.AssemblyExpression = assemblyExp;

            var moduleExp = new StartupModulesExpression();
            _moduleExpression?.Invoke(moduleExp);
            moduleExp.Build();
            objFactory.StartupModulesExpression = moduleExp;

            var overrideExp = new DefaultsExpression();
            _overrideExpression?.Invoke(overrideExp);
            objFactory.OverrideExpression = overrideExp;

            // if no assemblies have been configured follow the default scanner rule
            // will throw exception for netstandard1.0 applications
            var assembliesForStartup = GetDefaultAssemblies(useDiscoverableAssemblies, assemblyExp);
            ExecuteBuild(objFactory, assembliesForStartup, overrideExp, _usingImport);
            return this;
        }

        /// <summary>
        /// Enables Import&lt;T> when true is passed. 
        /// </summary>
        /// <param name="importEnabled"></param>
        /// <returns></returns>
        public StartupBuilder UseImport(bool importEnabled = true)
        {
            _usingImport = importEnabled;
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
        /// For manually adding/removing service from a given collection registrations
        /// </summary>
        /// <param name="registrationCollectionExpression"></param>
        /// <returns></returns>
        public StartupBuilder ConfigureRegistrations(Action<IRegistrationCollection> registrationCollectionExpression)
        {
            _registrationCollectionExpression += registrationCollectionExpression;
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
        public StartupBuilder OverrideDefaults(Action<DefaultsExpression> overrideExpression)
        {
            _overrideExpression += overrideExpression;
            return this;
        }

        /// <summary>
        /// Runs IStartupModules
        /// </summary>
        public void Run()
        {
            if (!_isConfigured) { Build(); }// just in case its not called fluently
            if (_runOnce) { return; }
            _runOnce = true;

            if (_startupHandler is null)
            {
                throw new Exception($"{nameof(Run)} was called but no startup handler was defined!");
            }

            _startupHandler.TryExecuteStartupModules();
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

        /// <summary>
        /// Add manual registrations if assigned
        /// <para>IMPORTANT: Must execute before module expression!</para>
        /// </summary>
        private void AddManualRegistrations()
        {
            if (_registrationCollectionExpression is object)
            {
                var collection = new Internal.RegistrationDescriptionCollection();
                _registrationCollectionExpression.Invoke(collection);

                if (collection.Any())
                {
                    ConfigureStartupModules(moduleExpression =>
                    {
                        moduleExpression.ConfigureLocatorModuleCollection(config =>
                            config.Add(new Internal.RegisterRegistrationDescriptionCollection(collection)));
                    });
                }
            }
        }

        private void ExecuteBuild(StartupBuilderObjectFactory objFactory, IEnumerable<Assembly> assemblies, DefaultsExpression defaults, bool enableImport)
        {
            var startupConfig = objFactory.CreateStartupConfiguration(assemblies);
            IStartupHandler localStartupHandlerFactory(IStartupConfiguration config) =>
                new StartupHandler(objFactory.CreateTimedTask, objFactory.CreateRegistryFactory(config), objFactory.CreateContainerDefaults(), objFactory.GetRegistryFinalizer(), enableImport: enableImport);
            _startupHandler = (defaults.StartupHandlerFactory ?? localStartupHandlerFactory)?.Invoke(startupConfig);

            StartupContext = _startupHandler?.ConfigureLocator(startupConfig);
        }

        private ICollection<Assembly> GetDefaultAssemblies(bool useDiscoverableAssemblies, AssemblyExpression assemblyExpression)
        {
            if (assemblyExpression.WithNoScanning) { return new List<Assembly>(); }
            if (assemblyExpression.Assemblies.Count > 0) { return assemblyExpression.Assemblies; }
            var defaultLoader = new Internal.AssemblyLoader();

            return useDiscoverableAssemblies ?
                AssemblyExpression.GetScannableAssemblies(defaultLoader.GetAssemblies()) :
                defaultLoader.GetAssemblies().ToList();
        }
    }
}