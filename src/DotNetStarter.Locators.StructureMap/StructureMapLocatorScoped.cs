using DotNetStarter.Abstractions;
using StructureMap;
using System;

namespace DotNetStarter.Locators
{
    /// <summary>
    /// Scoped structuremap locator
    /// </summary>
    public sealed class StructureMapLocatorScoped : StructureMapLocator, ILocatorScoped
    {
        private Action _onDispose;

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="container"></param>
        /// <param name="locator"></param>
        public StructureMapLocatorScoped(IContainer container, ILocator locator) : base(container) => Parent = locator as ILocatorScoped;

        /// <summary>
        /// Parent scope or null
        /// </summary>
        public ILocatorScoped Parent { get; }

        /// <summary>
        /// Action to perform on disposing
        /// </summary>
        /// <param name="disposeAction"></param>
        public void OnDispose(Action disposeAction) => _onDispose += disposeAction;

        /// <summary>
        /// Dispose ILocatorScoped
        /// </summary>
        public override void Dispose()
        {
            _onDispose?.Invoke();
            base.Dispose();
        }
    }
}