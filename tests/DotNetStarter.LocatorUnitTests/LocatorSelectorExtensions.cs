using DotNetStarter.Configure;

namespace DotNetStarter.UnitTests
{
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
#endif
            return builder;
        }
    }
}