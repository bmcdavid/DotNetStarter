using DotNetStarter.Abstractions;

[assembly: System.Runtime.InteropServices.ComVisible(false)]
[assembly: System.Runtime.InteropServices.Guid("f01c1501-b8f5-4b3d-b2a1-0975731992e6")]

[assembly: Exports(typeof(RegistrationConfiguration))] // only export this type
[assembly: DiscoverableAssembly]
[assembly: DiscoverTypes
(
    typeof(RegistrationAttribute),
    typeof(StartupModuleAttribute),
    typeof(IStartupModule)
)]