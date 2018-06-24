using DotNetStarter.Abstractions;
using DotNetStarter.Configure;
using DotNetStarter.Locators;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;

namespace DotNetStarter.StartupBuilderTests
{
    [TestClass]
    public partial class FluentTests
    {
        [TestMethod]
        public void ShouldAlwaysBeFirstToTestImportThenStartupAndResolveType()
        {
            // part 1, null Import
            Exception test = null;
            try
            {
                var sut = new TestFooImport().FooImport.Service;
            }
            catch(Exception e)
            {
                test = e;
            }

            Assert.IsTrue(test is NullReferenceException);

            // part 2
            var builder = StartupBuilder.Create();
            builder
                .ConfigureAssemblies(assemblies =>
                {
                    assemblies
                        .WithAssemblyFromType<RegistrationConfiguration>()
                        .WithAssembliesFromTypes(typeof(StartupBuilder), typeof(BadStartupModule));
                })
                .ConfigureStartupModules(modules =>
                {
                    modules
                        .RemoveStartupModule<BadStartupModule>()
                        .RemoveConfigureModule<BadConfigureModule>();
                })
                .UseEnvironment(new StartupEnvironment("UnitTest", AppDomain.CurrentDomain.BaseDirectory))
                .OverrideDefaults(defaults =>
                {
                    defaults
                        .UseLocatorRegistryFactory(new StructureMapFactory())
                        .UseLogger(new StringLogger(LogLevel.Info));
                })
                .BuildContainer(useApplicationContext: false)
                .Run();

            var logger = builder.StartupContext.Locator.Get<IStartupLogger>();

            Assert.IsNotNull(logger);
            Assert.IsFalse(ApplicationContext.Started);
            Assert.IsNotNull(new TestFooImport().FooImport.Service);
        }

        [TestMethod]
        public void ShouldRegisterModuleViaConfiguration()
        {
            var builder = StartupBuilder.Create();
            builder.
                ConfigureAssemblies(assemblies =>
                {
                    assemblies
                        .WithAssembliesFromTypes(typeof(StringLogger), typeof(RegistrationConfiguration));
                })
                .ConfigureStartupModules(modules =>
                {
                    modules.ConfigureStartupModuleCollection(collection =>
                    {
                        collection.AddType<TestStartupModule>();
                    })
                    .RemoveConfigureModule<BadConfigureModule>();
                })
                .OverrideDefaults(defaults =>
                {
                    defaults.UseLocatorRegistryFactory(new StructureMapFactory());
                })
                .UseEnvironment(new StartupEnvironment("UnitTest", ""))
                .BuildContainer(useApplicationContext: false)
                .Run();

            Assert.IsTrue(builder.StartupContext.Locator.Get<TestStartupModule>().Executed);
        }

        [TestMethod]
        public void ShouldStartupUsingAppContext()
        {
            var builder = StartupBuilder.Create();
            builder
                .ConfigureAssemblies(assemblies =>
                {
                    assemblies
                        .WithAssemblyFromType<RegistrationConfiguration>()
                        .WithAssembly(typeof(FluentTests).Assembly)
                        .WithAssembliesFromTypes(typeof(StartupBuilder));
                })
                .ConfigureStartupModules(modules =>
                {
                    modules
                        .RemoveStartupModule<BadStartupModule>()
                        .RemoveConfigureModule<BadConfigureModule>()
                        .RemoveConfigureModule<ConfigureTestFooService>();
                })
                .UseEnvironment(new StartupEnvironment("UnitTest", AppDomain.CurrentDomain.BaseDirectory))
                .OverrideDefaults(defaults =>
                {
                    defaults
                        .UseLocatorRegistryFactory(new StructureMapFactory())
                        .UseLogger(new StringLogger(LogLevel.Info));
                })
                .BuildContainer()
                .Run();

            var logger = builder.StartupContext.Locator.Get<IStartupLogger>();

            Assert.IsNotNull(logger);
            Assert.IsNotNull(ApplicationContext.Default);
            Assert.IsTrue(ApplicationContext.Started);
            Assert.AreEqual(builder.StartupContext, ApplicationContext.Default);
        }
    }
}