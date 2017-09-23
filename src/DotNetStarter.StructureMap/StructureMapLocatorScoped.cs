using DotNetStarter.Abstractions;
using StructureMap;

namespace DotNetStarter
{
    /// <summary>
    /// Scoped structuremap locator
    /// </summary>
    public class StructureMapLocatorScoped : StructureMapLocator, ILocatorScoped
    {
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="container"></param>
        public StructureMapLocatorScoped(IContainer container) : base(container)
        {
            IsActiveScope = true;
            Add(typeof(ILocator), this);
        }

        /// <summary>
        /// Denies access to base container
        /// </summary>
        public override object InternalContainer => throw new LocatorLockedException();

        /// <summary>
        /// Should always be true in scoped locator
        /// </summary>
        public bool IsActiveScope { get; }
    }
}