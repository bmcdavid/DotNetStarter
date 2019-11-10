namespace DotNetStarter.Abstractions
{
    using System;

    /// <summary>
    /// All dependency sorts derive from this and remember to create a constructor to pass dependencies
    /// <para>This base class supports Type sorting, but not Assembly</para>
    /// </summary>
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false, Inherited = true)]
    public abstract class StartupDependencyBaseAttribute : Attribute
    {
        /// <summary>
        /// Dependent types
        /// </summary>
        public Type[] Dependencies { get; }

        /// <summary>
        /// Determines if dependencies are assemblies and not types
        /// </summary>
        public bool IsAssemblyDependency { get; protected set; }

        private StartupDependencyBaseAttribute()
        {
        }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="dependencies"></param>
        public StartupDependencyBaseAttribute(params Type[] dependencies) : this()
        {
            Dependencies = dependencies;
            IsAssemblyDependency = false;
        }
    }
}