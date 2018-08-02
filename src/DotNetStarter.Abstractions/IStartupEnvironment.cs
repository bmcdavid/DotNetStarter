namespace DotNetStarter.Abstractions
{
    /// <summary>
    /// Information about current environment
    /// </summary>
    [CriticalComponent]
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
        /// Environment Items
        /// </summary>
        IItemCollection Items { get; }

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
        /// Determines if environment name is 'Local', typically used for a developer's local machine.
        /// </summary>
        /// <returns></returns>
        bool IsLocal();

        /// <summary>
        /// Determines if environment name is 'Production'
        /// </summary>
        /// <returns></returns>
        bool IsProduction();

        /// <summary>
        /// Determines if environment name is 'QualityAssurance'
        /// </summary>
        /// <returns></returns>
        bool IsQualityAssurance();

        /// <summary>
        /// Determines if environment name is 'Staging'
        /// </summary>
        /// <returns></returns>
        bool IsStaging();

        /// <summary>
        /// Determines if environment name is 'Testing'
        /// </summary>
        /// <returns></returns>
        bool IsTesting();

        /// <summary>
        /// Determines if environment name is 'UnitTest'
        /// </summary>
        /// <returns></returns>
        bool IsUnitTest();

        /// <summary>
        /// Determines if environment name is 'UserAcceptanceTesting'
        /// </summary>
        /// <returns></returns>
        bool IsUserAcceptanceTesting();
    }
}