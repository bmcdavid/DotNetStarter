using System;

namespace DotNetStarter.Abstractions
{
    /// <summary>
    /// Discover implementations/usages of these types in an assembly scanning process
    /// </summary>
    [AttributeUsage(AttributeTargets.Assembly, AllowMultiple = true, Inherited = false)]
    public class DiscoverTypesAttribute : Attribute
    {
        /// <summary>
        /// Types to discover.
        /// </summary>
        public Type[] DiscoverTypes { get; }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="discoverType">Types to discover for their implementation/usage.</param>
        public DiscoverTypesAttribute(params Type[] discoverType)
        {
            DiscoverTypes = discoverType;
        }
    }
}
