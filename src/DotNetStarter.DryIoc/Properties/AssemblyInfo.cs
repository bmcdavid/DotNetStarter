using System.Reflection;

#if !NETSTANDARD1_0

[assembly: System.Runtime.InteropServices.ComVisible(false)]
[assembly: System.Runtime.InteropServices.Guid("5a668531-c45d-45f6-8bbc-c2c924a8b237")]

#endif

[assembly: AssemblyTitle("DotNetStarter.DryIoc")]
[assembly: AssemblyDescription(".NET DI implementation using DryIoc.")]
[assembly: AssemblyProduct("DotNetStarter.DryIoc")]
[assembly: AssemblyCopyright("Copyright � {year}")]
[assembly: AssemblyVersion("1.1.2")]
[assembly: AssemblyFileVersion("1.1.2")]
[assembly: AssemblyInformationalVersion("1.1.2 Build: {build} Commit Hash: {commit}")]

[assembly: DotNetStarter.Abstractions.DotNetStarterScannableAssembly]
