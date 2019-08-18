using System;

namespace DotNetStarter.Abstractions
{
    /// <summary>
    /// Context keys for web frameworks
    /// </summary>
    public static class Keys
    {
        /// <summary>
        /// Dictionary Key to retrive scoped ILocator
        /// </summary>
        public static readonly string ScopedLocatorKeyInContext = $"{typeof(Keys).FullName}.{nameof(ILocator)}";

        /// <summary>
        /// Dictionary Key to retrive scoped IServiceProvider
        /// </summary>
        public static readonly string ScopedProviderKeyInContext = $"{typeof(Keys).FullName}.{nameof(IServiceProvider)}";
    }
}
