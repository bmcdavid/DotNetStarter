using DotNetStarter.Abstractions;

namespace DotNetStarter
{
    /// <summary>
    /// Default Startup Environment
    /// </summary>
    public class StartupEnvironment : IStartupEnvironment
    {
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="environmentName"></param>
        /// <param name="applicationBasePath"></param>
        public StartupEnvironment(string environmentName, string applicationBasePath)
        {
            EnvironmentName = environmentName ?? throw new System.ArgumentNullException(nameof(environmentName));
            ApplicationBasePath = applicationBasePath ?? string.Empty;
        }

        /// <summary>
        /// Base path of application, if known
        /// </summary>
        public virtual string ApplicationBasePath { get; }

        /// <summary>
        /// Current environment name
        /// </summary>
        public virtual string EnvironmentName { get; }

        /// <summary>
        /// Determines if environment name is 'Development'
        /// </summary>
        /// <returns></returns>
        public virtual bool IsDevelopment() => IsEnvironment("Development");

        /// <summary>
        /// Determines if given environment matches current environment.
        /// </summary>
        /// <param name="environment"></param>
        /// <returns></returns>
        public virtual bool IsEnvironment(string environment) => string.CompareOrdinal(EnvironmentName, environment) == 0;

        /// <summary>
        /// Determines if environment name is 'Local', typically used for a developer's local machine.
        /// </summary>
        /// <returns></returns>
        public virtual bool IsLocal() => IsEnvironment("Local");

        /// <summary>
        /// Determines if environment name is 'Production'
        /// </summary>
        /// <returns></returns>
        public virtual bool IsProduction() => IsEnvironment("Production");

        /// <summary>
        /// Determines if environment name is 'QualityAssurance'
        /// </summary>
        /// <returns></returns>
        public virtual bool IsQualityAssurance() => IsEnvironment("QualityAssurance");
        
        /// <summary>
        /// Determines if environment name is 'Staging'
        /// </summary>
        /// <returns></returns>
        public virtual bool IsStaging() => IsEnvironment("Staging");

        /// <summary>
        /// Determines if environment name is 'Testing'
        /// </summary>
        /// <returns></returns>
        public virtual bool IsTesting() => IsEnvironment("Testing");

        /// <summary>
        /// Determines if environment name is 'UserAcceptanceTesting'
        /// </summary>
        /// <returns></returns>
        public virtual bool IsUserAcceptanceTesting() => IsEnvironment("UserAcceptanceTesting");
    }
}
