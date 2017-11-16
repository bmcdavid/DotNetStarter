using DotNetStarter.Abstractions;
using StructureMap;

namespace DotNetStarter.Locators
{
    /// <summary>
    /// Scoped structuremap locator
    /// </summary>
    public sealed class StructureMapLocatorScoped : StructureMapLocatorBase, ILocatorScoped
    {
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="container"></param>
        /// <param name="locator"></param>
        public StructureMapLocatorScoped(IContainer container, ILocator locator) : base(container)
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