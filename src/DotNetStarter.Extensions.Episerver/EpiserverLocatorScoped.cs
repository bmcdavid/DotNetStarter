using DotNetStarter.Abstractions;
using DotNetStarter.Abstractions.Internal;
using StructureMap;

namespace DotNetStarter.Extensions.Episerver
{
    /// <summary>
    /// Scoped Episerver locator
    /// </summary>
    public class EpiserverLocatorScoped : EpiserverStructuremapLocator, ILocatorScoped
    {
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="container"></param>
        public EpiserverLocatorScoped(IContainer container) : base(container)
        {
            IsActiveScope = true;
            base.Add(typeof(ILocator), this);
        }

        /// <summary>
        /// Denies access to base container
        /// </summary>
        public override object InternalContainer => throw new LocatorLockedException();

        /// <summary>
        /// Should always be true in scope
        /// </summary>
        public bool IsActiveScope { get; }
    }
}