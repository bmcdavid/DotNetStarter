using DotNetStarter.Abstractions;
using DotNetStarter.Abstractions.Internal;
using DotNetStarter.Configure;
using DotNetStarter.UnitTests.Mocks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Linq;
using System.Text;

namespace DotNetStarter.UnitTests
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
        public void ShouldExecuteFromDefaults()
        {
            var b = StartupBuilder.Create();
            b.Build().Run();
            Assert.IsNotNull(b.StartupContext);
        }

        [ExpectedException(typeof(NotImplementedException))]
        [TestMethod]
        public void ShouldFailwithBadConfigureModule()
        {
            var builder = CreateTestBuilder();
            builder.ConfigureStartupModules(modules =>
            {
                modules.ConfigureLocatorModuleCollection(l =>
                {
                    l.Add(new BadConfigureModule());
                });
            }).Run();
        }

        [ExpectedException(typeof(NotImplementedException))]
        [TestMethod]
        public void ShouldFailwithBadStartupModule()
        {
            var builder = CreateTestBuilder();
            builder.ConfigureStartupModules(modules =>
            {
                modules.ConfigureStartupModuleCollection(s =>
                {
                    s.AddType<BadStartupModule>();
                });
            }).Run();
        }

        [TestMethod]
        public void ShouldRegisterConfigureModuleViaConfiguration()
        {
            var sut = new ManualLocatorConfigure();
            var builder = StartupBuilder.Create();
            builder
                .AddLocatorAssembly()
                .ConfigureAssemblies(assemblies =>
                {
                    assemblies
                        .WithAssembliesFromTypes(typeof(StringLogger), typeof(RegistrationConfiguration));
                })
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
        }

        [TestMethod]
        public void ShouldRegisterDescriptorCollection()
        {
            var builder = CreateTestBuilder();
            builder
                .ConfigureAssemblies(a => a.WithNoAssemblyScanning())
                .ConfigureRegistrations(r =>
                {
                    r.AddScoped<ILocatorScopedAccessor, LocatorScopedAccessor>();
                    r.AddSingleton<ILocatorAmbient, Internal.LocatorAmbient>();
                    r.Add(
                    RegistrationDescriptor.Singleton<ILocatorScopedFactory>(_ => new LocatorScopeFactory(_.GetService(typeof(ILocator)) as ILocator, _.GetService(typeof(ILocatorAmbient)) as ILocatorAmbient))
                    );
                    r.TryAddSingleton(new FooSingleton());
                    r.AddSingleton<FooSingleton, FooSingleton>();
                    r.TryAddTransient<FooTransient>();
                    r.AddTransient(typeof(FooTransient), typeof(FooTransient));
                    r.TryAddScoped<FooScoped>();
                    r.AddScoped<FooScoped, FooScoped>();
                    //todo: test more usages
                })
                .Build()
                .Run();

            var l = builder.StartupContext.Locator;
            var sut1 = l.Get<FooSingleton>();
            var sut2 = l.Get(typeof(FooSingleton));
            Assert.AreSame(sut1, sut2, "Singleton");

            var sut3 = l.Get<FooTransient>();
            var sut4 = l.Get(typeof(FooTransient));
            Assert.AreNotSame(sut3, sut4, "Transient");

            using (var scope = l.Get<ILocatorScopedFactory>().CreateScope())
            {
                sut3 = scope.Get<FooTransient>();
                sut4 = scope.Get(typeof(FooTransient));
                Assert.AreNotSame(sut3, sut4, "Transient In Scope");

                var sut5 = scope.Get<FooScoped>();
                var sut6 = scope.Get(typeof(FooScoped));

                Assert.AreSame(sut5, sut6, "Scoped");
            }
        }

        [TestMethod]
        public void ShouldRegisterModuleViaConfiguration()
        {
            var builder = CreateTestBuilder();
            builder
                .AddLocatorAssembly()
                .ConfigureAssemblies(assemblies =>
                {
                    assemblies
                        .WithDiscoverableAssemblies(new[] { typeof(StringLogger).Assembly(), typeof(RegistrationConfiguration).Assembly() })
                        .WithAssemblyFromType<BadConfigureModule>();
                })
                .ConfigureStartupModules(modules =>
                {
                    modules
                        .ConfigureStartupModuleCollection(collection =>
                        {
                            collection.AddType<TestStartupModule>();
                        });
                })
                .Build()
                .Run();

            var sut = builder.StartupContext.Locator.GetAll<IStartupModule>().FirstOrDefault(f => f is TestStartupModule) as TestStartupModule;
            Assert.IsTrue(sut.Executed);
        }

        [TestMethod]
        public void ShouldRemoveBadModules()
        {
            var builder = CreateTestBuilder();
            builder
                .ConfigureStartupModules(modules =>
                {
                    modules.ConfigureLocatorModuleCollection(l =>
                    {
                        l.Add(new BadConfigureModule());
                    })
                    .ConfigureStartupModuleCollection(s =>
                    {
                        s.AddType<BadStartupModule>();
                    });
                })
                .ConfigureStartupModules(modules =>
                {
                    modules
                    .RemoveConfigureModule<BadConfigureModule>()
                    .RemoveStartupModule<BadStartupModule>();
                })
                .Run();
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
        public void ShouldStartupAndResolveTypeAndImport()
        {
            var builder = CreateTestBuilder();
            builder
                .UseImport()
                .ConfigureAssemblies(assemblies =>
                {
                    assemblies
                        .WithAssemblyFromType<RegistrationConfiguration>()
                        .WithAssembliesFromTypes(typeof(StartupBuilder), typeof(BadStartupModule));
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
            Assert.IsNotNull(new TestFooImport().FooImport.Service);
            // ran when test assembly is initialized
            Assert.IsTrue(TestSetup.TestImport is NullReferenceException);
        }

        [TestMethod]
        public void ShouldStartupUsingStaticContext()
        {
            var builder = CreateTestBuilder();
            builder
                .UseStatic()
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
                        .ConfigureLocatorModuleCollection(m => m.Add(new BadStaticTest()))
                        .RemoveConfigureModule<ConfigureTestFooService>();
                })
                .OverrideDefaults(defaults =>
                {
                    defaults
                        .UseLogger(new StringLogger(LogLevel.Info));
                })
                .Run(); // omitting build for default

            builder.Build().Run(); // 2nd pass shouldn't do anything

            var logger = builder.StartupContext.Locator.Get<IStartupLogger>();
            Assert.IsTrue(BadStaticTest.Executed);
            Assert.IsNotNull(logger);
            Assert.IsNotNull(builder.StartupContext.AsStatic());
            Assert.AreEqual(builder.StartupContext, ((IStartupContext)null).AsStatic());
        }

        [TestMethod]
        public void ShouldThrowNoHandlerException()
        {
            bool failed = false;
            try
            {
                var builder = CreateTestBuilder();
                builder
                    .ConfigureAssemblies(a => a.WithNoAssemblyScanning())
                    .OverrideDefaults(d => d.UseStartupHandler(c => null))
                    .Build()
                    .Run();
            }
            catch (NullStartupHandlerException)
            {
                failed = true;
            }

            Assert.IsTrue(failed);
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
            .UseTestLocator();
    }
}