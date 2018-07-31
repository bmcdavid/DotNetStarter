using DotNetStarter.Abstractions;

#if !NETSTANDARD1_0
[assembly: System.Runtime.InteropServices.ComVisible(false)]
[assembly: System.Runtime.InteropServices.Guid("ba813dc5-d696-461d-8e62-f04fcd669adb")]
#endif

[assembly: DiscoverableAssembly]
[assembly: System.Runtime.CompilerServices.InternalsVisibleTo("DotNetStarter.StartupBuilderTests, PublicKey=" + PubKeys.PublicKey)]

internal class PubKeys
{
    //sn.exe path C:\Program Files (x86)\Microsoft SDKs\Windows\v7.0A\Bin
    internal const string PublicKey = "002400000480000094000000060200000024000052534131000400000100010099d6c09b10a1e95bf97895023e81ec85b8b9dfc67c3ac842f6c146f1becee5b57db8251076fd4a181d36452c9a2730e38d8eecbdb641bb70378b2b1d2e0e15c3d4a44ac8a4ffdf8a6edc02f58fc1d14103d3ec5eb5384ffb7456abf23f0bf2d69ae229c0a13e30bb31e1fdfdb1e449a67c3885befe135b6e86b2011229117bb1";

    internal const string PublicKeyToken = "b0eb17a6888ebfb7";
}