using DotNetStarter.Abstractions;
using DotNetStarter.Configure;
using DotNetStarter.UnitTests.Mocks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Diagnostics.CodeAnalysis;

namespace DotNetStarter.UnitTests
{
    [ExcludeFromCodeCoverage]
    [TestClass]
    public sealed class TestSetup
    {
        internal static Exception TestImport { get; private set; }

        internal static IStartupContext TestContext { get; private set; }

        [AssemblyInitialize]
        public static void Setup(TestContext _)
        {
            try
            {
                var sut = new TestFooImport().FooImport.Service;
            }
            catch (Exception e)
            {
                TestImport = e;
            }

            var builder = StartupBuilder.Create();

            builder
                .UseImport()
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