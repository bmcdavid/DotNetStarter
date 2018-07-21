using System;

#if !NETSTANDARD1_0 && !NETSTANDARD1_1 && !NETSTANDARD1_3

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
            AppDomain.CurrentDomain.DomainUnload -= CurrentDomain_DomainUnload;
        }

        void IStartupModule.Startup(IStartupEngine engine)
        {
            _ShutdownHandler = engine.Locator.Get<IShutdownHandler>(); // cannot inject it, to avoid recursion
            AppDomain.CurrentDomain.DomainUnload -= CurrentDomain_DomainUnload;
            AppDomain.CurrentDomain.DomainUnload += CurrentDomain_DomainUnload;
        }

        void CurrentDomain_DomainUnload(object sender, EventArgs e)
        {
            _ShutdownHandler.Shutdown();
        }
    }
}

#endif
