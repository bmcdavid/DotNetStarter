using DotNetStarter.Abstractions;
using DotNetStarter.Abstractions.Internal;
using System;

namespace DotNetStarter
{
    /// <summary>
    /// Default ILocatorScopeFactory implementation
    /// </summary>
    [Registration(typeof(ILocatorScopedFactory), Lifecycle.Singleton)]
    public class LocatorScopeFactory : ILocatorScopedFactory
    {
        private readonly ILocator _locator;
        private readonly ILocatorAmbient _locatorAmbient;

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="unscopedLocator"></param>
        /// <param name="locatorAmbient"></param>
        public LocatorScopeFactory(ILocator unscopedLocator, ILocatorAmbient locatorAmbient)
        {
            _locator = unscopedLocator;
            _locatorAmbient = locatorAmbient;
        }

        /// <summary>
        /// Creates a child scope of given scoped locator
        /// </summary>
        /// <param name="locatorScoped"></param>
        /// <returns></returns>
        public virtual ILocatorScoped CreateChildScope(ILocatorScoped locatorScoped)
        {
            return _Create(locatorScoped);
        }

        /// <summary>
        /// Creates an initial scope
        /// </summary>
        /// <returns></returns>
        public virtual ILocatorScoped CreateScope()
        {
            return _Create(_locator);
        }

        private ILocatorScoped _Create(ILocator locator)
        {
            if (!(locator is ILocatorCreateScope creator))
            {
                throw new ArgumentException($"{locator.GetType().FullName} doesn't implement {typeof(ILocatorCreateScope).FullName}!");
            }

            var scope = creator.CreateScope();
            var accessor = scope.Get<ILocatorScopedAccessor>();

            if (!(accessor is ILocatorScopedSetter setter))
            {
                throw new Exception($"{accessor.GetType().FullName} must implement {typeof(ILocatorScopedSetter).FullName}!");
            }

            setter.SetCurrentScopedLocator(scope);

#if !NETSTANDARD1_0 && !NETSTANDARD1_1
            if (_locatorAmbient is ILocatorAmbientWithSet settable)
            {
                if (!(scope is ILocatorScopedWithDisposeAction disposeAction))
                {
                    throw new ArgumentException($"{scope.GetType().FullName} must implement {typeof(ILocatorScopedWithDisposeAction).FullName}!");
                }

                disposeAction.OnDispose(() => settable.SetCurrentScopedLocator(null));
                settable.SetCurrentScopedLocator(scope);
            }
#endif

            return scope;
        }
    }
}