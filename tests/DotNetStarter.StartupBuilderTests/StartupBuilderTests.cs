using DotNetStarter.Abstractions;
using DotNetStarter.Abstractions.Internal;
using DotNetStarter.Configure;
using DotNetStarter.Locators;
using DotNetStarter.StartupBuilderTests.Mocks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Linq;
using System.Text;

namespace DotNetStarter.StartupBuilderTests
{
    [TestClass]
    public class StartupBuilderTests
    {
        [TestMethod]
        public void ShouldChangeRegistrationLifecycle()
        {
            var builder = CreateTestBuilder();
            builder
                .ConfigureAssemblies(assemblies =>
                {
                    assemblies
                        .WithAssemblyFromType<RegistrationConfiguration>()
                        .WithAssembliesFromTypes(typeof(StartupBuilder), typeof(TestFooImport));
                })
                .ConfigureStartupModules(modules =>
                {
                    modules
                        .RemoveStartupModule<BadStartupModule>()
                        .RemoveConfigureModule<BadConfigureModule>();
                })
                .OverrideDefaults(defaults =>
                {
                    defaults
                        .UseRegistrationModifier(new MockRegistrationModifier())
                        .UseLogger(new StringLogger(LogLevel.Info));
                })
                .Build()
                .Run();

            var sut1 = builder.StartupContext.Locator.Get<TestFooImport>();
            var sut2 = builder.StartupContext.Locator.Get<TestFooImport>();

            Assert.AreSame(sut1, sut2);
            Assert.AreEqual(sut1.DateTime, sut2.DateTime);
        }

        [TestMethod]
        public void ShouldRegisterDescriptorCollection()
        {
            var builder = CreateTestBuilder();
            builder
                .ConfigureAssemblies(a => a.WithNoAssemblyScanning())
                .ConfigureRegistrations(r =>
                {
                    r.TryAddSingleton(new TestFooImport());
                    r.AddSingleton<TestFooImport, TestFooImport>();
                    //todo: test more usages
                })
                .Build()
                .Run();

            var sut1 = builder.StartupContext.Locator.Get<TestFooImport>();
            var sut2 = builder.StartupContext.Locator.Get<TestFooImport>();

            Assert.AreSame(sut1, sut2);
        }

        [TestMethod]
        public void ShouldExecuteFromDefaults()
        {
            // todo: fix or remove
            // only using discoverable assemblies to remove bad modules for unit testing
            //StartupBuilder.Create().Build(useDiscoverableAssemblies: true).Run();
            //Assert.IsNotNull(ApplicationContext.Default);
            //Internal.UnitTestHelper.ResetApplication();
        }
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
                .Build()
                .Run();

            Assert.IsTrue(sut.Executed);
        }

        [TestMethod]
        public void ShouldRegisterModuleViaConfiguration()
        {
            var builder = CreateTestBuilder();
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
                .Build()
                .Run();

            var sut = builder.StartupContext.Locator.GetAll<IStartupModule>().OfType<TestStartupModule>().FirstOrDefault();
            Assert.IsTrue(sut.Executed);
        }

        [TestMethod]
        public void ShouldRunWithNoScanning()
        {
            var sut = new ManualLocatorConfigure();
            var builder = CreateTestBuilder();
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
                .Build()
                .Run();

            Assert.IsTrue(sut.Executed);
            Assert.IsFalse(builder.StartupContext.Configuration.Assemblies.Any());
        }

        [TestMethod]
        public void ShouldStartupAndResolveType()
        {
            var builder = CreateTestBuilder();
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
                .OverrideDefaults(defaults =>
                {
                    defaults
                        .UseLogger(new StringLogger(LogLevel.Info));
                })
                .Build()
                .Run();

            builder.Build().Run();
            var logger = builder.StartupContext.Locator.Get<IStartupLogger>();
            Assert.IsTrue(builder.StartupContext.Configuration.Environment.IsEnvironment("UnitTest1"));
            Assert.IsNotNull(logger);
            //Assert.IsNotNull(new TestFooImport().FooImport.Service);
            // ran when test assembly is initialized
            Assert.IsTrue(SetupTests.TestImport is NullReferenceException);
        }

        [TestMethod]
        public void ShouldStartupUsingAppContext()
        {
            //todo: fix
            //Assert.IsFalse(ApplicationContext.Started);

            //var builder = CreateTestBuilder();
            //builder
            //    .ConfigureAssemblies(assemblies =>
            //    {
            //        assemblies
            //            .WithAssemblyFromType<RegistrationConfiguration>()
            //            .WithAssembly(typeof(StartupBuilderTests).Assembly())
            //            .WithAssembliesFromTypes(typeof(StartupBuilder));
            //    })
            //    .ConfigureStartupModules(modules =>
            //    {
            //        modules
            //            .RemoveStartupModule<BadStartupModule>()
            //            .RemoveConfigureModule<BadConfigureModule>()
            //            .RemoveConfigureModule<ConfigureTestFooService>();
            //    })
            //    .OverrideDefaults(defaults =>
            //    {
            //        defaults
            //            .UseLogger(new StringLogger(LogLevel.Info));
            //    })
            //    .Run(); // omitting build for default

            //builder.Build().Run(); // 2nd pass shouldn't do anything

            //var logger = builder.StartupContext.Locator.Get<IStartupLogger>();
            //Assert.IsNotNull(logger);
            //Assert.IsNotNull(ApplicationContext.Default);
            //Assert.IsTrue(ApplicationContext.Started);
            //Assert.AreEqual(builder.StartupContext, ApplicationContext.Default);
            //Internal.UnitTestHelper.ResetApplication();
        }

        [TestMethod]
        public void ShouldThrowErrorAccessingStaticContextDuringInit()
        {
            //todo: fix or remove
            //string sut = string.Empty;
            //try
            //{
            //    var builder = CreateTestBuilder();
            //    builder
            //        .ConfigureAssemblies(assemblies => assemblies.WithNoAssemblyScanning())
            //        .ConfigureStartupModules(modules =>
            //        {
            //            modules
            //                .ConfigureLocatorModuleCollection(c => c.Add(new FaultyAccessStaticDuringStartup()))
            //                .RemoveStartupModule<BadStartupModule>()
            //                .RemoveConfigureModule<BadConfigureModule>();
            //        })
            //        .Run();
            //}
            //catch (Exception e)
            //{
            //    sut = e.Message;
            //}

            //Assert.IsTrue(sut.StartsWith("Do not access"));
        }

        [TestMethod]
        public void ShouldThrowNoHandlerException()
        {
            string sut = string.Empty;
            try
            {
                var builder = CreateTestBuilder();
                builder
                    .ConfigureAssemblies(a => a.WithNoAssemblyScanning())
                    .OverrideDefaults(d => d.UseStartupHandler(c => null))
                    .Build()
                    .Run();
            }
            catch (Exception e)
            {
                sut = e.Message;
            }

            Assert.IsTrue(sut.Contains("was called but no startup handler was defined"));
        }
        [TestMethod]
        public void ShouldUseEnvironmentItemsAcrossModules()
        {
            var builder = CreateTestBuilder();
            builder
                .ConfigureAssemblies(a => a.WithNoAssemblyScanning())
                .ConfigureStartupModules(modules =>
                {
                    modules
                        .ConfigureLocatorModuleCollection(l => l.Add(new ConfigureModuleAddEnvironmentItem()))
                        .ConfigureStartupModuleCollection(s => s.AddType<StartupModuleUseEnvironmentItem>());
                })
                .OverrideDefaults(defaults =>
                {
                    defaults
                        .UseRegistrationModifier(new MockRegistrationModifier())
                        .UseLogger(new StringLogger(LogLevel.Info));
                })
                .Build()
                .Run();

            var sut = builder.StartupContext.Configuration.Environment.Items.Get<StringBuilder>().ToString().Split(new string[] { Environment.NewLine }, StringSplitOptions.RemoveEmptyEntries).ToList();

            Assert.IsTrue(sut.Count == 2);
            Assert.IsTrue(sut[0] == "Configured Item!");
            Assert.IsTrue(sut[1] == "Started Item!");
        }

        private StartupBuilder CreateTestBuilder() => StartupBuilder.Create()
            .UseEnvironment(new StartupEnvironment("UnitTest1", ""))
            .OverrideDefaults(d => d.UseLocatorRegistryFactory(new DryIocLocatorFactory()));
    }
}