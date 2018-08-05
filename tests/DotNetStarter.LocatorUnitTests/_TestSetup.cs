using DotNetStarter.Abstractions;
using DotNetStarter.Configure;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace DotNetStarter.UnitTests
{
    [TestClass]
    public sealed class _TestSetup
    {
        [AssemblyInitialize]
        public static void Setup(TestContext context)
        {
            var builder = StartupBuilder.Create();

            builder.ConfigureAssemblies(assemblies =>
            {
                assemblies
                .WithAssemblyFromType<Mocks.FooService>()
                .WithAssemblyFromType<StartupBuilder>()
                .WithAssemblyFromType<RegistrationConfiguration>();
            });

            builder
                .AddLocatorAssembly()
                .Build()
                .Run();
        }
    }
}