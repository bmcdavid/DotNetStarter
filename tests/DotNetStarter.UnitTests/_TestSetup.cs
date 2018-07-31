using DotNetStarter.Abstractions;
using DotNetStarter.Abstractions.Internal;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using System.Reflection;

[assembly: DiscoverableAssembly]

namespace DotNetStarter.UnitTests
{
    [TestClass]
    public sealed class _TestSetup
    {
        [AssemblyInitialize]
        public static void Setup(TestContext context)
        {
            List<Assembly> assemblies = new List<Assembly>
            {
                typeof(DotNetStarter.ApplicationContext).Assembly(),
                typeof(DotNetStarter.Abstractions.RegistrationConfiguration).Assembly(),
                typeof(DotNetStarter.UnitTests.Mocks.ExcludeModule).Assembly()
            };

            DotNetStarter.Configure.StartupBuilder.Create()
                .ConfigureAssemblies(a => a.WithAssemblies(assemblies))
                .ConfigureStartupModules(m => m.RemoveStartupModule<Mocks.ExcludeModule>())
                .OverrideDefaults(d => 
                {
                    d
                        .UseLocatorRegistryFactory(new Mocks.TestLocatorFactory())
                        .UseAssemblyFilter(new Mocks.TestAssemblyFilter())
                        .UseLogger(new Mocks.TestLogger());
                })
                .Run();
        }
    }
}