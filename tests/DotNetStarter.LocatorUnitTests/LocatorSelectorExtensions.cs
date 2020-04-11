using DotNetStarter.Configure;
using System.Diagnostics.CodeAnalysis;

namespace DotNetStarter.UnitTests
{
    [ExcludeFromCodeCoverage]
    public static class LocatorBuilderExtensiosn
    {
        public static StartupBuilder AddLocatorAssembly(this StartupBuilder builder)
        {
#if DRYIOC_LOCATOR
            builder.ConfigureAssemblies(a => a.WithAssemblyFromType<DotNetStarter.Locators.DryIocLocatorFactory>());
#elif STRUCTUREMAP_LOCATOR
            builder.ConfigureAssemblies(a => a.WithAssemblyFromType<DotNetStarter.Locators.StructureMapFactory>());
#elif STRUCTUREMAPSIGNED_LOCATOR
            builder.ConfigureAssemblies(a => a.WithAssemblyFromType<DotNetStarter.Locators.StructureMapSignedFactory>());
#elif LIGHTINJECT_LOCATOR
            builder.ConfigureAssemblies(a => a.WithAssemblyFromType<DotNetStarter.Locators.LightInjectLocatorRegistryFactory>());
#elif GRACE_LOCATOR
            builder.ConfigureAssemblies(a => a.WithAssemblyFromType<DotNetStarter.Locators.GraceLocatorRegistryFactory>());
#elif LAMAR_LOCATOR
            builder.ConfigureAssemblies(a => a.WithAssemblyFromType<DotNetStarter.Locators.LamarLocatorRegistryFactory>());
#elif AUTOFAC_LOCATOR
            builder.ConfigureAssemblies(a => a.WithAssemblyFromType<DotNetStarter.Locators.AutofacLocatorRegistryFactory>());
#elif STASHBOX_LOCATOR
            builder.ConfigureAssemblies(a => a.WithAssemblyFromType<DotNetStarter.Locators.StashboxLocatorRegistryFactory>());
#else
            throw new Exception("Container not setup for tests!");
#endif
            return builder;
        }

        public static StartupBuilder UseTestLocator(this StartupBuilder builder)
        {
#if DRYIOC_LOCATOR
            builder.OverrideDefaults(d => d.UseLocatorRegistryFactory(new DotNetStarter.Locators.DryIocLocatorFactory()));
#elif STRUCTUREMAP_LOCATOR
            builder.OverrideDefaults(d => d.UseLocatorRegistryFactory(new DotNetStarter.Locators.StructureMapFactory()));
#elif STRUCTUREMAPSIGNED_LOCATOR
            builder.OverrideDefaults(d => d.UseLocatorRegistryFactory(new DotNetStarter.Locators.StructureMapSignedFactory()));
#elif LIGHTINJECT_LOCATOR
            builder.OverrideDefaults(d => d.UseLocatorRegistryFactory(new DotNetStarter.Locators.LightInjectLocatorRegistryFactory()));
#elif GRACE_LOCATOR
            builder.OverrideDefaults(d => d.UseLocatorRegistryFactory(new DotNetStarter.Locators.GraceLocatorRegistryFactory()));
#elif LAMAR_LOCATOR
            builder.OverrideDefaults(d => d.UseLocatorRegistryFactory(new DotNetStarter.Locators.LamarLocatorRegistryFactory()));
#elif AUTOFAC_LOCATOR
            builder.OverrideDefaults(d => d.UseLocatorRegistryFactory(new DotNetStarter.Locators.AutofacLocatorRegistryFactory()));
#elif STASHBOX_LOCATOR
            builder.OverrideDefaults(d => d.UseLocatorRegistryFactory(new DotNetStarter.Locators.StashboxLocatorRegistryFactory()));
#else
            throw new Exception("Container not setup for tests!");
#endif
            return builder;
        }
    }
}