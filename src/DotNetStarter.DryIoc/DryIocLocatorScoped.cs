namespace DotNetStarter
{
    using DotNetStarter.Abstractions;
    using DotNetStarter.Abstractions.Internal;
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
            this.SetCurrentScopedLocator();

            // Critical component to replace application ILocator with scoped one
            container.RegisterDelegate(typeof(ILocator), resolver => this, Reuse.Singleton);
            container.RegisterDelegate(typeof(ILocatorScoped), resolver => this, Reuse.Singleton);
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
