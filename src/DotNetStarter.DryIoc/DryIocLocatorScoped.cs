namespace DotNetStarter
{
    using DotNetStarter.Abstractions;
    using DryIoc;

    /// <summary>
    /// Scoped DryIoc locator
    /// </summary>
    public sealed class DryIocLocatorScoped : DryIocLocatorBase, ILocatorScoped
    {
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="container"></param>
        /// <param name="locator"></param>
        public DryIocLocatorScoped(IContainer container, ILocator locator) : base(container)
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