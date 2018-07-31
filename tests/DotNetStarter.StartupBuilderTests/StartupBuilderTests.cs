using DotNetStarter.Abstractions;
using DotNetStarter.Abstractions.Internal;
using DotNetStarter.Configure;
using DotNetStarter.Locators;
using DotNetStarter.StartupBuilderTests.Mocks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Linq;

namespace DotNetStarter.StartupBuilderTests
{
    [TestClass]
    public class StartupBuilderTests
    {
        [TestMethod]
        public void ShouldRegisterConfigureModuleViaConfiguration()
        {
            var sut = new ManualLocatorConfigure();
            var builder = StartupBuilder.Create();
            builder
                .ConfigureAssemblies(assemblies =>
                {
                    assemblies
                        .WithAssembliesFromTypes(typeof(StringLogger), typeof(RegistrationConfiguration))
                        .WithAssemblyFromType<DryIocLocatorFactory>();
                })
                .ConfigureStartupModules(modules =>
                {
                    modules
                        .ConfigureLocatorModuleCollection(configureModules =>
                        {
                            configureModules.Add(sut);
                        })
                        .RemoveConfigureModule<BadConfigureModule>();
                })
                .Build(useApplicationContext: false)
                .Run();

            Assert.IsTrue(sut.Executed);
        }

        [TestMethod]
        public void ShouldRegisterModuleViaConfiguration()
        {
            var builder = StartupBuilder.Create();
            builder
                .ConfigureAssemblies(assemblies =>
                {
                    assemblies
                        .WithDiscoverableAssemblies(new[] { typeof(StringLogger).Assembly(), typeof(RegistrationConfiguration).Assembly() })
                        .WithAssemblyFromType<DryIocLocatorFactory>()
                        .WithAssemblyFromType<BadConfigureModule>();
                })
                .ConfigureStartupModules(modules =>
                {
                    modules
                        .ConfigureStartupModuleCollection(collection =>
                        {
                            collection.AddType<TestStartupModule>();
                        })
                        .RemoveConfigureModule<BadConfigureModule>()
                        .RemoveStartupModule<BadStartupModule>()
                        ;
                })
                .Build(useApplicationContext: false)
                .Run();

            var sut = builder.StartupContext.Locator.GetAll<IStartupModule>().OfType<TestStartupModule>().FirstOrDefault();
            Assert.IsTrue(sut.Executed);
        }

        [TestMethod]
        public void ShouldRunFromDefaults()
        {
            // only using discoverable assemblies to remove bad modules for unit testing
            StartupBuilder.Create().Build(useDiscoverableAssemblies: true).Run();
            Assert.IsNotNull(ApplicationContext.Default);
            Internal.UnitTestHelper.ResetApplication();
        }

        [TestMethod]
        public void ShouldRunWithNoScanning()
        {
            var sut = new ManualLocatorConfigure();
            var builder = StartupBuilder.Create();
            builder
                .ConfigureAssemblies(a => a.WithNoAssemblyScanning())
                .ConfigureStartupModules(modules =>
                {
                    modules
                        .ConfigureLocatorModuleCollection(configureModules =>
                        {
                            configureModules.Add(sut);
                        });
                })
                .OverrideDefaults(d => d.UseLocatorRegistryFactory(new DryIocLocatorFactory()))
                .Build(useApplicationContext: false)
                .Run();

            Assert.IsTrue(sut.Executed);
            Assert.IsFalse(builder.StartupContext.Configuration.Assemblies.Any());
        }

        [TestMethod]
        public void ShouldStartupAndResolveType()
        {
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
                .UseEnvironment(new StartupEnvironment("UnitTest1", ""))
                .OverrideDefaults(defaults =>
                {
                    defaults
                        .UseLocatorRegistryFactory(new DryIocLocatorFactory())
                        .UseLogger(new StringLogger(LogLevel.Info));
                })
                .Build(useApplicationContext: false)
                .Run();

            var logger = builder.StartupContext.Locator.Get<IStartupLogger>();

            Assert.IsTrue(builder.StartupContext.Configuration.Environment.IsEnvironment("UnitTest1"));
            Assert.IsNotNull(logger);
            Assert.IsNotNull(new TestFooImport().FooImport.Service);
            // ran when test assembly is initialized
            Assert.IsTrue(SetupTests.TestImport is NullReferenceException);
        }

        [TestMethod]
        public void ShouldStartupUsingAppContext()
        {
            Assert.IsFalse(ApplicationContext.Started);

            var builder = StartupBuilder.Create();
            builder
                .ConfigureAssemblies(assemblies =>
                {
                    assemblies
                        .WithAssemblyFromType<RegistrationConfiguration>()
                        .WithAssembly(typeof(StartupBuilderTests).Assembly())
                        .WithAssembliesFromTypes(typeof(StartupBuilder));
                })
                .ConfigureStartupModules(modules =>
                {
                    modules
                        .RemoveStartupModule<BadStartupModule>()
                        .RemoveConfigureModule<BadConfigureModule>()
                        .RemoveConfigureModule<ConfigureTestFooService>();
                })
                .OverrideDefaults(defaults =>
                {
                    defaults
                        .UseLocatorRegistryFactory(new DryIocLocatorFactory())
                        .UseLogger(new StringLogger(LogLevel.Info));
                })
                .Build()
                .Run();

            var logger = builder.StartupContext.Locator.Get<IStartupLogger>();
            Assert.IsNotNull(logger);
            Assert.IsNotNull(ApplicationContext.Default);
            Assert.IsTrue(ApplicationContext.Started);
            Assert.AreEqual(builder.StartupContext, ApplicationContext.Default);
            Internal.UnitTestHelper.ResetApplication();
        }
    }
}