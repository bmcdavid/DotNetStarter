using DotNetStarter.Abstractions;
using DryIoc;

namespace DotNetStarter
{
    /// <summary>
    /// Scoped DryIoc locator
    /// </summary>
    public class DryIocLocatorScoped : DryIocLocator, ILocatorScoped
    {
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="container"></param>
        public DryIocLocatorScoped(IContainer container) : base(container)
        {
            IsActiveScope = true;
            base.Add(typeof(ILocator), this);
        }

        /// <summary>
        /// Denies access to base container
        /// </summary>
        public override object InternalContainer => throw new LocatorLockedException();

        /// <summary>
        /// Should always be true in scoped containers
        /// </summary>
        public bool IsActiveScope { get; }
    }
}
