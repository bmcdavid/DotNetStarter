using System;

namespace DotNetStarter.Internal
{
    /// <summary>
    /// Assemblies not set exception
    /// </summary>
    public class AssembliesNotConfiguredException : Exception
    {
        /// <summary>
        /// Constructor
        /// </summary>
        public AssembliesNotConfiguredException() :
            base($"No assemblies were configured for scanning! Please add them using the {nameof(Configure.StartupBuilder.ConfigureAssemblies)} callback in {typeof(DotNetStarter.Configure.StartupBuilder).FullName} API!")
        {
        }
    }
}
