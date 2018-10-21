using DotNetStarter.Abstractions;
using DotNetStarter.Configure;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace DotNetStarter.UnitTests
{
    [TestClass]
    public sealed class _TestSetup
    {
        internal static IStartupContext TestContext { get; private set; }

        [AssemblyInitialize]
        public static void Setup(TestContext context)
        {
            var builder = StartupBuilder.Create();

            builder
                .UseEnvironment(new UnitTestEnvironment())
                .ConfigureStartupModules(x => x.RemoveStartupModule<Mocks.ExcludeModule>())
                .ConfigureAssemblies(assemblies =>
                {
                    assemblies
                    .WithAssemblyFromType<Mocks.FooService>()
                    .WithAssemblyFromType<StartupBuilder>()
                    .WithAssemblyFromType<RegistrationConfiguration>();
                })
                .OverrideDefaults(d =>
                {
                    d
                    .UseAssemblyFilter(new Mocks.TestAssemblyFilter())
                    .UseLogger(new Mocks.TestLogger());
                })
                .AddLocatorAssembly()
                .Build()
                .Run();

            TestContext = builder.StartupContext;
        }
    }
}