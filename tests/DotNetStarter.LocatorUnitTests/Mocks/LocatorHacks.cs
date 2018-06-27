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
            var lightInjectContainer = engine.Locator.InternalContainer as LightInject.IServiceContainer;

            if (lightInjectContainer != null)
            {
                // hack: needed for injecting func params
                lightInjectContainer.RegisterConstructorDependency((factory, info, runtimeArgs) => (IInjectable)(runtimeArgs[0]));
            }

#endif
        }
    }
}