namespace DotNetStarter.Abstractions
{
    using System;

    //todo: v2, remove this

    /// <summary>
    /// All dependency sorts derive from this and remember to create a constructor to pass dependencies
    /// <para>This base class supports Type sorting, but not Assembly</para>
    /// </summary>
    [Obsolete("Will be replaced by StartupDependencyBaseAttribute in v2.")]
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false, Inherited = true)]
    public abstract class DependencyBaseAttribute : Attribute
    {
        /// <summary>
        /// Dependent types
        /// </summary>
        public Type[] Dependencies { get; }

        /// <summary>
        /// Determines if dependencies are assemblies and not types
        /// </summary>
        public bool IsAssemblyDependency { get; protected set; }

        [Obsolete("Please use the params Type[] constructor", true)]
        private DependencyBaseAttribute() { }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="dependencies"></param>
        public DependencyBaseAttribute(params Type[] dependencies)
        {
            Dependencies = dependencies;
            IsAssemblyDependency = false;
        }
    }
}