namespace DotNetStarter.Abstractions
{
    using System;

    /// <summary>
    /// Sets a dependency for startup modules
    /// </summary>    
    public class StartupModuleAttribute : DependencyBaseAttribute
    {
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="moduleDependencies">Modules required to run before this module.</param>
        public StartupModuleAttribute(params Type[] moduleDependencies) : base(moduleDependencies)
        {
        }
    }
}
