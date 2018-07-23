using System;

namespace DotNetStarter.Abstractions
{
    /// <summary>
    /// Sets locator's container
    /// </summary>
    [Obsolete]
    public interface ILocatorSetContainer
    {
        /// <summary>
        /// Sets locator's container
        /// </summary>
        /// <param name="container"></param>
        void SetContainer(object container);
    }
}
