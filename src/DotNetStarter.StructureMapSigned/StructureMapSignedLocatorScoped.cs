using DotNetStarter.Abstractions;
using StructureMap;

namespace DotNetStarter
{
    /// <summary>
    /// Scoped structuremap locator
    /// </summary>
    public sealed class StructureMapSignedLocatorScoped : StructureMapSignedLocatorBase, ILocatorScoped
    {
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="container"></param>
        /// <param name="locator"></param>
        public StructureMapSignedLocatorScoped(IContainer container, ILocator locator) : base(container)
        {
            Parent = locator as ILocatorScoped;
        }

        /// <summary>
        /// Denies access to base container
        /// </summary>
        public override object InternalContainer => throw new LocatorLockedException();

        /// <summary>
        /// Parent scope or null
        /// </summary>
        public ILocatorScoped Parent { get; }
    }
}