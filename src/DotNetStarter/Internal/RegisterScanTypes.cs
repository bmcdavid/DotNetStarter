using DotNetStarter.Abstractions;

/*
 * This is how to assign types to get scanned on startup
 */

[assembly: ScanTypeRegistry(
    typeof(IStartupModule),
    typeof(StartupModuleAttribute),
    typeof(RegisterAttribute)
)]