using DotNetStarter.Abstractions;
using DotNetStarter.Abstractions.Internal;
using StructureMap;

namespace DotNetStarter
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
        /// <param name="scopeKind"></param>
        public StructureMapLocatorScoped(IContainer container, ILocator locator, IScopeKind scopeKind) : base(container)
        {
            Parent = locator as ILocatorScoped;
            ScopeKind = scopeKind;

            // Critical component to replace application ILocator with scoped one
            container.Configure(x =>
            {
                x.For(typeof(ILocator)).Singleton().Use(this);
                x.For(typeof(ILocatorScoped)).Singleton().Use(this);
            });
        }

        /// <summary>
        /// Denies access to base container
        /// </summary>
        public override object InternalContainer => throw new LocatorLockedException();

        /// <summary>
        /// Parent scope or null
        /// </summary>
        public ILocatorScoped Parent { get; }

        /// <summary>
        /// Scope kind
        /// </summary>
        public IScopeKind ScopeKind { get; }
    }
}