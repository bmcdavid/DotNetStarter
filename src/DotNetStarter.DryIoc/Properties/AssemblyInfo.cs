using System.Reflection;

#if !NETSTANDARD1_0

[assembly: System.Runtime.InteropServices.ComVisible(false)]
[assembly: System.Runtime.InteropServices.Guid("5a668531-c45d-45f6-8bbc-c2c924a8b237")]
#endif

[assembly: AssemblyTitle("DotNetStarter.DryIoc")]
[assembly: AssemblyDescription(".NET DI implementation using DryIoc.")]
[assembly: AssemblyProduct("DotNetStarter.DryIoc")]
[assembly: AssemblyCopyright("Copyright © {year}")]
[assembly: AssemblyVersion("2.0.0")]
[assembly: AssemblyFileVersion("2.0.0")]
[assembly: AssemblyInformationalVersion("2.0.0-alpha001 Build: {build} Commit Hash: {commit}")]

[assembly: DotNetStarter.Abstractions.DiscoverableAssembly]
[assembly: DotNetStarter.Abstractions.Exports] // no need to export any types
[assembly: DotNetStarter.Abstractions.LocatorRegistryFactory(typeof(DotNetStarter.DryIocLocatorFactory))]