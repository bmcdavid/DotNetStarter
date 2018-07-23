using DotNetStarter.Abstractions;

namespace DotNetStarter.UnitTests
{
    [StartupModule]
    public class LocatorHacks : IStartupModule
    {
        void IStartupModule.Shutdown()
        {
        }

        void IStartupModule.Startup(IStartupEngine engine)
        {
#if LIGHTINJECT_LOCATOR
            if (engine.Locator.InternalContainer is LightInject.IServiceContainer lightInjectContainer)
            {
                // hack: needed for injecting func params
                lightInjectContainer.RegisterConstructorDependency((factory, info, runtimeArgs) => (IInjectable)(runtimeArgs[0]));
            }

#endif
        }
    }
}