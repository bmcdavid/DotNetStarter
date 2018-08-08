using DotNetStarter.Abstractions;

namespace DotNetStarter.Internal
{
    /// <summary>
    /// Allows for application context to be reset, use ONLY in unit test projects
    /// </summary>
    public class UnitTestHelper
    {
        private static readonly object _objLock = new object();

        /// <summary>
        /// Reset is for unit test purposes
        /// </summary>
        public static void ResetApplication()
        {
            lock (_objLock)
            {
                ApplicationContext._Default = null;
                ApplicationContext.Started = false;
                AssemblyLoader.LoadedAssemblies.Clear();
            }
        }

        /// <summary>
        /// For testing purposes
        /// </summary>
        /// <param name="started"></param>
        public static void SetApplicationStarted(bool started) => ApplicationContext.Started = started;

        /// <summary>
        /// For testing purposes
        /// </summary>
        /// <param name="startupContext"></param>
        public static void SetApplicationDefaultContext(IStartupContext startupContext) => ApplicationContext._Default = startupContext;
    }
}