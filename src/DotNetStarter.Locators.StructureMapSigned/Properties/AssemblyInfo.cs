[assembly: DotNetStarter.Abstractions.DiscoverableAssembly]
[assembly: DotNetStarter.Abstractions.Exports] // no need to export any types
[assembly: DotNetStarter.Abstractions.LocatorRegistryFactory(typeof(DotNetStarter.Locators.StructureMapSignedFactory))]