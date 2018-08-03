using DotNetStarter.Abstractions;
using System;
#if LAMAR_LOCATOR || NETSTANDARD
using Microsoft.Extensions.DependencyInjection;
#endif
namespace DotNetStarter.UnitTests
{
    [StartupModule]
    public class LocatorHacks : ILocatorConfigure
    {
        public void Configure(ILocatorRegistry registry, ILocatorConfigureEngine configArgs)
        {
            configArgs.OnStartupComplete += () =>
            {
                // hack: needed for injecting func params
#if LIGHTINJECT_LOCATOR
                if (registry.InternalContainer is LightInject.IServiceContainer lightInjectContainer)
                {
                    lightInjectContainer.RegisterConstructorDependency((factory, info, runtimeArgs) => (IInjectable)(runtimeArgs[0]));
                }
#elif LAMAR_LOCATOR
                if (registry.InternalContainer is Lamar.Container lamarContainer)
                {
                    lamarContainer.Configure(c =>
                    {
                        c.AddTransient((provider) =>
                        {
                            return new Func<IInjectable, TestFuncCreationComplex>(
                                (i) => new TestFuncCreationComplex(i,
                                provider.GetService(typeof(IStartupConfiguration)) as IStartupConfiguration,
                                provider.GetService(typeof(IShutdownHandler)) as IShutdownHandler)
                            );
                        });
                    });
                }
#endif
            };
        }
    }
}