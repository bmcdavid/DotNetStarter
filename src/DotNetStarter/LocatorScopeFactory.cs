﻿using DotNetStarter.Abstractions;
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
            if (!(locator is ILocatorWithCreateScope creator))
            {
                throw new ArgumentException($"{locator.GetType().FullName} doesn't implement {typeof(ILocatorWithCreateScope).FullName}!");
            }

            var scope = creator.CreateScope();
            var accessor = scope.Get<ILocatorScopedAccessor>();

            if (!(accessor is ILocatorScopedWithSet setter))
            {
                throw new Exception($"{accessor.GetType().FullName} must implement {typeof(ILocatorScopedWithSet).FullName}!");
            }

            setter.SetCurrentScopedLocator(scope);

            if (_locatorAmbient is ILocatorAmbientWithSet settable)
            {
                scope.OnDispose(() => settable.SetCurrentScopedLocator(null));
                settable.SetCurrentScopedLocator(scope);
            }

            return scope;
        }
    }
}