using System.Runtime.InteropServices;

[assembly: ComVisible(false)]
[assembly: Guid("db0d1573-a384-4d85-acc4-81127bdd9f42")]

[assembly: DotNetStarter.Abstractions.DiscoverableAssembly]
[assembly: DotNetStarter.Abstractions.Exports(DotNetStarter.Abstractions.ExportsType.ExportsOnly)]
[assembly: DotNetStarter.Abstractions.DiscoverTypes(typeof(System.Web.Http.ApiController))]