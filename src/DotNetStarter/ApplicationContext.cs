namespace DotNetStarter
{
    using Abstractions;
    using DotNetStarter.Configure;
    using System;

    /// <summary>
    /// Provides access to application static IStartupContext.
    /// </summary>
    public class ApplicationContext
    {
        /// <summary>
        /// Dictionary Key to retrive scoped IServiceProvider
        /// </summary>
        public static readonly string ScopedProviderKeyInContext = typeof(ApplicationContext).FullName + "." + nameof(IServiceProvider);

        /// <summary>
        /// Dictionary Key to retrive scoped ILocator
        /// </summary>
        public static readonly string ScopedLocatorKeyInContext = typeof(ApplicationContext).FullName + "." + nameof(ILocator);

        /// <summary>
        /// Used to determine if application default startup has executed
        /// </summary>
        public static bool Started { get; internal set; }

        internal static IStartupContext _Default;

        private ApplicationContext() { }

        /// <summary>
        /// Default context instance
        /// </summary>
        public static IStartupContext Default
        {
            get
            {
                //todo: should a default be ran here? or throw exception to note startup hasn't been ran!
                if (_Default == null) { StartupBuilder.Create().Run(); }

                return _Default;
            }
        }
    }
}