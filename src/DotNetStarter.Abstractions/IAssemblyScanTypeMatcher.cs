namespace DotNetStarter.Abstractions
{
    using System;

    /// <summary>
    /// Determines if scan type matches registered item
    /// </summary>
    public interface IAssemblyScanTypeMatcher
    {
        /// <summary>
        /// Determines if scan type matches registered item
        /// </summary>
        /// <param name="registeredType"></param>
        /// <param name="scannedType"></param>
        /// <returns></returns>
        bool IsMatch(Type registeredType, Type scannedType);
    }
}
