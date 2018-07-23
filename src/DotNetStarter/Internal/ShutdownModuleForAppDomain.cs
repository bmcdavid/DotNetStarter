namespace DotNetStarter.Abstractions.Internal
{
    /// <summary>
    /// Shutdown module to ensure shutdown for netframework
    /// </summary>
    [StartupModule(typeof(RegistrationConfiguration))]
    public class ShutdownModuleForAppDomain : IStartupModule
    {
        IShutdownHandler _ShutdownHandler;

        void IStartupModule.Shutdown()
        {
#if NET35 || NET40 || NET45 || NETSTANDARD2_0
            System.AppDomain.CurrentDomain.DomainUnload -= (s,e) => CurrentDomain_DomainUnload();
#elif NETSTANDARD1_6
            System.Runtime.Loader.AssemblyLoadContext.Default.Unloading -= (context) => CurrentDomain_DomainUnload();
#endif
        }

        void IStartupModule.Startup(IStartupEngine engine)
        {
            _ShutdownHandler = engine.Locator.Get<IShutdownHandler>(); // cannot inject it, to avoid recursion

#if NET35 || NET40 || NET45 || NETSTANDARD2_0
            System.AppDomain.CurrentDomain.DomainUnload -= (s, e) => CurrentDomain_DomainUnload();
            System.AppDomain.CurrentDomain.DomainUnload += (s, e) => CurrentDomain_DomainUnload();
#elif NETSTANDARD1_6
            System.Runtime.Loader.AssemblyLoadContext.Default.Unloading -= (context) => CurrentDomain_DomainUnload();
            System.Runtime.Loader.AssemblyLoadContext.Default.Unloading += (context) => CurrentDomain_DomainUnload();
#endif
        }

        void CurrentDomain_DomainUnload()
        {
            _ShutdownHandler.Shutdown();
        }
    }
}