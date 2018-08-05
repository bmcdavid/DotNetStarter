namespace DotNetStarter.Abstractions.Internal
{
    /// <summary>
    /// Shutdown module to ensure shutdown for netframework
    /// </summary>
    [StartupModule(typeof(RegistrationConfiguration))]
    public class ShutdownModuleForAppDomain : IStartupModule
    {
        private IShutdownHandler _ShutdownHandler;

        void IStartupModule.Shutdown()
        {
#if HAS_APP_DOMAIN
            System.AppDomain.CurrentDomain.DomainUnload -= (s, e) => CurrentDomain_DomainUnload();
#elif HAS_ASSEMBLY_LOAD_CONTEXT
            System.Runtime.Loader.AssemblyLoadContext.Default.Unloading -= (context) => CurrentDomain_DomainUnload();
#endif
        }

        void IStartupModule.Startup(IStartupEngine engine)
        {
            _ShutdownHandler = engine.Locator.Get<IShutdownHandler>(); // cannot inject it, to avoid recursion

#if HAS_APP_DOMAIN
            System.AppDomain.CurrentDomain.DomainUnload -= (s, e) => CurrentDomain_DomainUnload();
            System.AppDomain.CurrentDomain.DomainUnload += (s, e) => CurrentDomain_DomainUnload();
#elif HAS_ASSEMBLY_LOAD_CONTEXT
            System.Runtime.Loader.AssemblyLoadContext.Default.Unloading -= (context) => CurrentDomain_DomainUnload();
            System.Runtime.Loader.AssemblyLoadContext.Default.Unloading += (context) => CurrentDomain_DomainUnload();
#endif
        }

        private void CurrentDomain_DomainUnload()
        {
            _ShutdownHandler.Shutdown();
        }
    }
}