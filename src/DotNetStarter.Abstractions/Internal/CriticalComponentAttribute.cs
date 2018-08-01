using System;

namespace DotNetStarter.Abstractions
{
    /// <summary>
    /// Denotes an API that should never break as its usage is spread across many NuGetPackages and creates a lot of headaches.
    /// <para>Strongly avoid removing or changing method signatures, properties, etc</para>
    /// <para>Add with extreme caution to avoid breaking changes.</para>
    /// </summary>
    [AttributeUsage(AttributeTargets.Interface|AttributeTargets.Class)]
    public class CriticalComponentAttribute : Attribute
    {
    }
}
