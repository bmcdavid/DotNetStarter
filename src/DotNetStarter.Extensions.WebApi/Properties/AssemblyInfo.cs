using System.Reflection;
using System.Runtime.InteropServices;

[assembly: ComVisible(false)]
[assembly: Guid("db0d1573-a384-4d85-acc4-81127bdd9f42")]

[assembly: AssemblyTitle("DotNetStarter.Extensions.WebApi")]
[assembly: AssemblyDescription(".NET startup system for netframework WebApi")]
[assembly: AssemblyProduct("DotNetStarter.Extensions.WebApi")]
[assembly: AssemblyCopyright("Copyright © {year}")]
[assembly: AssemblyVersion("2.0.0")]
[assembly: AssemblyFileVersion("2.0.0")]
[assembly: AssemblyInformationalVersion("2.0.0-alpha001 Build: {build} Commit Hash: {commit}")]

[assembly: DotNetStarter.Abstractions.DiscoverableAssembly]
[assembly: DotNetStarter.Abstractions.Exports(DotNetStarter.Abstractions.ExportsType.ExportsOnly)]
[assembly: DotNetStarter.Abstractions.DiscoverTypes(typeof(System.Web.Http.ApiController))]