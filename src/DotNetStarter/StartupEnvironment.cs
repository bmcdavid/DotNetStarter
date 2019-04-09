using DotNetStarter.Abstractions;

namespace DotNetStarter
{
    /// <summary>
    /// Default Startup Environment
    /// </summary>
    public class StartupEnvironment : IStartupEnvironment
    {
        private bool? _IsLocal, _IsDev, _IsProd, _IsStage, _IsQA, _IsUAT, _IsTest, _IsUnitTest;

        /// <summary>
        /// UnitTest Environment name
        /// </summary>
        protected const string UnitTestName = "UnitTest";

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="environmentName"></param>
        /// <param name="applicationBasePath"></param>
        /// <param name="defaultItems"></param>
        public StartupEnvironment(string environmentName, string applicationBasePath = null, IItemCollection defaultItems = null)
        {
            EnvironmentName = environmentName ?? throw new System.ArgumentNullException(nameof(environmentName));
            ApplicationBasePath = applicationBasePath ?? string.Empty;
            Items = defaultItems ?? new StartupEnvironmentItemCollection();
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
        /// Current environment items
        /// </summary>
        public IItemCollection Items { get; }

        /// <summary>
        /// Determines if environment name is 'Development'
        /// </summary>
        /// <returns></returns>
        public virtual bool IsDevelopment()
        {
            if (_IsDev is null)
            {
                _IsDev = IsEnvironment("Development");
            }

            return _IsDev.Value;
        }

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
        public virtual bool IsLocal()
        {
            if (_IsLocal is null)
            {
                _IsLocal = IsEnvironment("Local");
            }

            return _IsLocal.Value;
        }

        /// <summary>
        /// Determines if environment name is 'Production'
        /// </summary>
        /// <returns></returns>
        public virtual bool IsProduction()
        {
            if (_IsProd is null)
            {
                _IsProd = IsEnvironment("Production");
            }

            return _IsProd.Value;
        }

        /// <summary>
        /// Determines if environment name is 'QualityAssurance'
        /// </summary>
        /// <returns></returns>
        public virtual bool IsQualityAssurance()
        {
            if (_IsQA is null)
            {
                _IsQA = IsEnvironment("QualityAssurance");
            }

            return _IsQA.Value;
        }

        /// <summary>
        /// Determines if environment name is 'Staging'
        /// </summary>
        /// <returns></returns>
        public virtual bool IsStaging()
        {
            if (_IsStage is null)
            {
                _IsStage = IsEnvironment("Staging");
            }

            return _IsStage.Value;
        }

        /// <summary>
        /// Determines if environment name is 'Testing'
        /// </summary>
        /// <returns></returns>
        public virtual bool IsTesting()
        {
            if (_IsTest is null)
            {
                _IsTest = IsEnvironment("Testing");
            }

            return _IsTest.Value;
        }

        /// <summary>
        /// Determines if environment name is 'UnitTest'
        /// </summary>
        /// <returns></returns>
        public bool IsUnitTest()
        {
            if (_IsUnitTest is null)
            {
                _IsUnitTest = IsEnvironment(UnitTestName);
            }

            return _IsUnitTest.Value;
        }

        /// <summary>
        /// Determines if environment name is 'UserAcceptanceTesting'
        /// </summary>
        /// <returns></returns>
        public virtual bool IsUserAcceptanceTesting()
        {
            if (_IsUAT is null)
            {
                _IsUAT = IsEnvironment("UserAcceptanceTesting");
            }

            return _IsUAT.Value;
        }
    }
}