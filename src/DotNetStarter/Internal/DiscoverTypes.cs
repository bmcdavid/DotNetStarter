using DotNetStarter.Abstractions;

// Look for implementations/usages of these types

[assembly: DiscoverTypes(
    typeof(RegistrationAttribute),
    typeof(IStartupModule),
    typeof(StartupModuleAttribute),
    typeof(RegisterAttribute)
)]