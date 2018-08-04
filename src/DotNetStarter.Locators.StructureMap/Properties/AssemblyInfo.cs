using System.Runtime.InteropServices;

[assembly: ComVisible(false)]
[assembly: Guid("b9852d14-ec6f-47fb-888c-75c238bc9a06")]
[assembly: DotNetStarter.Abstractions.DiscoverableAssembly]
[assembly: DotNetStarter.Abstractions.Exports] // no need to export any types
[assembly: DotNetStarter.Abstractions.LocatorRegistryFactory(typeof(DotNetStarter.Locators.StructureMapFactory))]