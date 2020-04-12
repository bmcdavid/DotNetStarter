using DotNetStarter.Abstractions;
using DotNetStarter.UnitTests.Mocks;
using System.Diagnostics.CodeAnalysis;

#if LAMAR_LOCATOR || NETSTANDARD
using Microsoft.Extensions.DependencyInjection;
#endif

namespace DotNetStarter.UnitTests
{
    [ExcludeFromCodeCoverage]
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
                            return new System.Func<IInjectable, TestFuncCreationComplex>(
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