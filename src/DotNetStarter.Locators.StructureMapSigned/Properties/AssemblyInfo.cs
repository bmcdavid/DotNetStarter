using System.Reflection;

[assembly: AssemblyTitle("DotNetStarter.Locators.StructureMapSigned")]
[assembly: AssemblyDescription(".NET DI implementation using structuremap.")]
[assembly: AssemblyProduct("DotNetStarter.Locators.StructureMapSigned")]
[assembly: AssemblyCopyright("Copyright � {year}")]
[assembly: AssemblyVersion("1.0.0")]
[assembly: AssemblyFileVersion("1.0.0")]
[assembly: AssemblyInformationalVersion("1.0.0 Build: {build} Commit Hash: {commit}")]

[assembly: DotNetStarter.Abstractions.DiscoverableAssembly]
[assembly: DotNetStarter.Abstractions.Exports] // no need to export any types
[assembly: DotNetStarter.Abstractions.LocatorRegistryFactory(typeof(DotNetStarter.Locators.StructureMapSignedFactory))]