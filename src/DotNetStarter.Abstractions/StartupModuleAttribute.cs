namespace DotNetStarter.Abstractions
{
    using System;

    /// <summary>
    /// Required for IStartupModule and ILocatorConfigure implementations to execute, and optionally define dependencies
    /// </summary>    
    public class StartupModuleAttribute : StartupDependencyBaseAttribute
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
