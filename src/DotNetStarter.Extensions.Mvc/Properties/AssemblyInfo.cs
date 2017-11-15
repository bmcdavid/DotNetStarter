using System.Reflection;
using System.Runtime.InteropServices;

[assembly: ComVisible(false)]
[assembly: Guid("5f0d3088-4b29-4ecd-8df0-c5f14366db5b")]

[assembly: AssemblyTitle("DotNetStarter.Extensions.Mvc")]
[assembly: AssemblyDescription(".NET startup system for netframework MVC.")]
[assembly: AssemblyProduct("DotNetStarter.Extensions.Mvc")]
[assembly: AssemblyCopyright("Copyright © {year}")]
[assembly: AssemblyVersion("2.0.0")]
[assembly: AssemblyFileVersion("2.0.0")]
[assembly: AssemblyInformationalVersion("2.0.0-alpha001 Build: {build} Commit Hash: {commit}")]

[assembly: DotNetStarter.Abstractions.DiscoverableAssembly]
//[assembly: DotNetStarter.Abstractions.Exports(DotNetStarter.Abstractions.ExportsType.ExportsOnly)]
[assembly: DotNetStarter.Abstractions.DiscoverTypes(typeof(System.Web.Mvc.IController))]
