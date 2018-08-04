using System;

namespace DotNetStarter.Abstractions
{
    /// <summary>
    /// Provides type checking for service providers
    /// </summary>
    public interface IServiceProviderTypeChecker
    {
        /// <summary>
        /// Determines if given type is in a DotNetStarter scanned assembly. If true, the implementation should throw exception.
        /// </summary>
        /// <param name="type"></param>
        /// <param name="exception"></param>
        /// <returns></returns>
        bool IsScannedAssembly(Type type, Exception exception);
    }
}