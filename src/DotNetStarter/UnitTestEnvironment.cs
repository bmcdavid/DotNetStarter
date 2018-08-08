namespace DotNetStarter
{
    /// <summary>
    /// Simple Environment for unit testing where environment name is 'UnitTest'
    /// </summary>
    public class UnitTestEnvironment : StartupEnvironment
    {
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="environmentName">UnitTest by default</param>
        /// <param name="applicationBasePath"></param>
        public UnitTestEnvironment(string environmentName = UnitTestName, string applicationBasePath = null) :
            base(environmentName, applicationBasePath)
        {

        }
    }
}