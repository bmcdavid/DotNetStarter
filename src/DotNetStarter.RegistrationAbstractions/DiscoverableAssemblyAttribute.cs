namespace DotNetStarter.Abstractions
{
    using System;

    /// <summary>
    /// When assigned to assemblies, libraries such as DotNetStarter can use it for easier assembly filtering.
    /// </summary>
    [AttributeUsage(AttributeTargets.Assembly, AllowMultiple = false)]
    public class DiscoverableAssemblyAttribute : Attribute { }
}
