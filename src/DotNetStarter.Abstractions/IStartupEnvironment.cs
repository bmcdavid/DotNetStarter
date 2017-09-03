namespace DotNetStarter.Abstractions
{
    /// <summary>
    /// Information about current environment
    /// </summary>
    public interface IStartupEnvironment
    {
        /// <summary>
        /// Base path of application, if known
        /// </summary>
        string ApplicationBasePath { get; }

        /// <summary>
        /// Current environment name
        /// </summary>
        string EnvironmentName { get; }

        /// <summary>
        /// Determines if environment name is 'Development'
        /// </summary>
        /// <returns></returns>
        bool IsDevelopment();

        /// <summary>
        /// Determines if given environment matches current environment.
        /// </summary>
        /// <param name="environment"></param>
        /// <returns></returns>
        bool IsEnvironment(string environment);

        /// <summary>
        /// Determines if environment name is 'Production'
        /// </summary>
        /// <returns></returns>
        bool IsProduction();

        /// <summary>
        /// Determines if environment name is 'Staging'
        /// </summary>
        /// <returns></returns>
        bool IsStaging();

        /// <summary>
        /// Determines if environment name is 'Test'
        /// </summary>
        /// <returns></returns>
        bool IsTest();
    }
}
