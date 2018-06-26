using System.Reflection;
using System.Runtime.InteropServices;

[assembly: ComVisible(false)]
[assembly: Guid("b9852d14-ec6f-47fb-888c-75c238bc9a06")]
[assembly: AssemblyTitle("DotNetStarter.StructureMap")]
[assembly: AssemblyDescription(".NET DI implementation using structuremap.")]
[assembly: AssemblyProduct("DotNetStarter.StructureMap")]
[assembly: AssemblyCopyright("Copyright � {year}")]
[assembly: AssemblyVersion("2.0.0")]
[assembly: AssemblyFileVersion("2.0.0")]
[assembly: AssemblyInformationalVersion("2.1.0 Build: {build} Commit Hash: {commit}")]

[assembly: DotNetStarter.Abstractions.DiscoverableAssembly]
[assembly: DotNetStarter.Abstractions.Exports] // no need to export any types
[assembly: DotNetStarter.Abstractions.LocatorRegistryFactory(typeof(DotNetStarter.Locators.StructureMapFactory))]