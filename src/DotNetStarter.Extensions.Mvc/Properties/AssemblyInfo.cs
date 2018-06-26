using System.Runtime.InteropServices;

[assembly: ComVisible(false)]
[assembly: Guid("5f0d3088-4b29-4ecd-8df0-c5f14366db5b")]

[assembly: DotNetStarter.Abstractions.DiscoverableAssembly]
[assembly: DotNetStarter.Abstractions.Exports(DotNetStarter.Abstractions.ExportsType.ExportsOnly)]
[assembly: DotNetStarter.Abstractions.DiscoverTypes(typeof(System.Web.Mvc.IController))]
