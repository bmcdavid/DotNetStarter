﻿namespace DotNetStarter.Internal
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
    }
}