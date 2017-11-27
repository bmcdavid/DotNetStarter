using DotNetStarter.Abstractions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Linq;

namespace DotNetStarter.Tests
{
    [TestClass]
    public sealed class _TestSetup
    {
        [AssemblyInitialize]
        public static void Setup(TestContext context)
        {
#if DRYNETSTANDARD
            var assemblies = System.AppDomain.CurrentDomain.GetAssemblies().ToList();
            assemblies.Add(typeof(DotNetStarter.Locators.DryIocLocator).Assembly);

            ApplicationContext.Startup(assemblies: assemblies);
#elif MAPNETSTANDARD
            var assemblies = System.AppDomain.CurrentDomain.GetAssemblies().ToList();
            assemblies.Add(typeof(DotNetStarter.Locators.StructureMapFactory).Assembly);

            ApplicationContext.Startup(assemblies: assemblies);
#else
            ApplicationContext.Startup();

#endif

        }
    }

    [StartupModule]
    public class LocatorHacks : IStartupModule
    {
        void IStartupModule.Shutdown()
        {
        }

        void IStartupModule.Startup(IStartupEngine engine)
        {
#if LIGHTINJECT
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
