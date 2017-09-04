using System;

[assembly: DotNetStarter.Abstractions.DotNetStarterScannableAssembly]

namespace DotNetStarter.Abstractions
{
    /// <summary>
    /// Assign to assemblies for easier assembly filtering.
    /// </summary>
    [AttributeUsage(AttributeTargets.Assembly, AllowMultiple = false)]
    public class DotNetStarterScannableAssemblyAttribute : Attribute { }
}
