using DotNetStarter.Abstractions;
using System.Reflection;

#if !NETSTANDARD1_0

[assembly: System.Runtime.InteropServices.ComVisible(false)]
[assembly: System.Runtime.InteropServices.Guid("ba813dc5-d696-461d-8e62-f04fcd669adb")]

#endif

// todo: v2 freeze versions on major except for informational

[assembly: AssemblyTitle("DotNetStarter")]
[assembly: AssemblyDescription(".NET startup system with dependency injection.")]
[assembly: AssemblyProduct("DotNetStarter")]
[assembly: AssemblyCopyright("Copyright © {year}")]
[assembly: AssemblyVersion("1.4.0")]
[assembly: AssemblyFileVersion("1.4.0")]
[assembly: AssemblyInformationalVersion("1.4.0 Build: {build} Commit Hash: {commit}")]

[assembly: DiscoverableAssembly]
[assembly: DiscoverTypes
(
    typeof(RegisterAttribute),
    typeof(RegistrationAttribute),
    typeof(StartupModuleAttribute),
    typeof(IStartupModule)
)]