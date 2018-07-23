using DotNetStarter.Abstractions;
//todo: can i remove this interopservices stuff, and also netstandard1.1
#if !NETSTANDARD1_0
[assembly: System.Runtime.InteropServices.ComVisible(false)]
[assembly: System.Runtime.InteropServices.Guid("f01c1501-b8f5-4b3d-b2a1-0975731992e6")]
#endif

[assembly: Exports(typeof(RegistrationConfiguration))] // only export this type
[assembly: DiscoverableAssembly]
[assembly: DiscoverTypes
(
    typeof(RegistrationAttribute),
    typeof(StartupModuleAttribute),
    typeof(IStartupModule)
)]