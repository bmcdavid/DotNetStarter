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
        /// Dictionary Key to retrive scoped ILocator
        /// </summary>
        public static readonly string ScopedLocatorKeyInContext = typeof(ApplicationContext).FullName + "." + nameof(ILocator);

        /// <summary>
        /// Dictionary Key to retrive scoped IServiceProvider
        /// </summary>
        public static readonly string ScopedProviderKeyInContext = typeof(ApplicationContext).FullName + "." + nameof(IServiceProvider);

        internal static IStartupContext _Default;

        private ApplicationContext() { }
    }
}