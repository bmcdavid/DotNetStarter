using DotNetStarter.Abstractions;

namespace DotNetStarter.UnitTests
{
    [StartupModule]
    public class LocatorHacks : ILocatorConfigure
    {
        public void Configure(ILocatorRegistry registry, ILocatorConfigureEngine configArgs)
        {
            configArgs.OnStartupComplete += () =>
            {
#if LIGHTINJECT_LOCATOR
                if (registry.InternalContainer is LightInject.IServiceContainer lightInjectContainer)
                {
                    // hack: needed for injecting func params
                    lightInjectContainer.RegisterConstructorDependency((factory, info, runtimeArgs) => (IInjectable)(runtimeArgs[0]));
                }
#endif
            };
        }
    }
}