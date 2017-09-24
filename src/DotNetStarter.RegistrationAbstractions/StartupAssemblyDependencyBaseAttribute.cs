namespace DotNetStarter.Abstractions
{
    using System;

    /// <summary>
    /// Sort Assemblies, the attribute requires you to use a type in the dependent assembly
    /// </summary>
    [AttributeUsage(AttributeTargets.Assembly, AllowMultiple = false, Inherited = true)]
    public abstract class StartupAssemblyDependencyBaseAttribute : StartupDependencyBaseAttribute
    {
        [Obsolete("Please use the params Type[] constructor", true)]
        private StartupAssemblyDependencyBaseAttribute() { }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="dependencies"></param>
        public StartupAssemblyDependencyBaseAttribute(params Type[] dependencies) : base(dependencies)
        {
            IsAssemblyDependency = true;
        }
    }
}
