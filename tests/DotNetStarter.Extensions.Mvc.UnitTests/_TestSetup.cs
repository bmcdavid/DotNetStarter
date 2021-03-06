﻿using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Reflection;

namespace DotNetStarter.Extensions.Mvc.Tests
{
    [TestClass]
    public class _TestSetup
    {
        [AssemblyInitialize]
        public static void Setup(TestContext context)
        {
            var scannableAssemblies = new Assembly[]
            {
                typeof(DotNetStarter.Abstractions.IAssemblyFilter).Assembly,
                typeof(DotNetStarter.ApplicationContext).Assembly,
                typeof(DotNetStarter.Locators.DryIocLocatorRegistry).Assembly,
                typeof(DotNetStarter.Web.Startup).Assembly,
                typeof(DotNetStarter.Extensions.Mvc.StartupMvc).Assembly,
                typeof(_TestSetup).Assembly
            };

            DotNetStarter.Configure.StartupBuilder.Create()
                .ConfigureAssemblies(a => a.WithAssemblies(scannableAssemblies))
                //.ConfigureAssemblies(a => a.WithDiscoverableAssemblies())
                .OverrideDefaults(d => d.UseLocatorRegistryFactory(new DotNetStarter.Locators.DryIocLocatorFactory()))
                .Build()
                .Run();
        }
    }
}