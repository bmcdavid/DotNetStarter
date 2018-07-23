#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member

using DotNetStarter.Abstractions;
using System;
using System.Collections.Generic;

namespace DotNetStarter.Internal
{
    public sealed class ReadOnlyLocator : IReadOnlyLocator, ILocatorCreateScope, ILocatorWithPropertyInjection
    {
        private readonly ILocator _ConfiguredLocator;

        private ReadOnlyLocator(ILocator configuredLocator)
        {
            _ConfiguredLocator = configuredLocator;
        }

        public string DebugInfo => _ConfiguredLocator.DebugInfo;

        public object InternalContainer
        {
            get
            {
                ThrowIfLocked();

                return _ConfiguredLocator.InternalContainer;
            }
        }

        public bool IsLocked { get; private set; } = false;

        public static ReadOnlyLocator CreateReadOnlyLocator(ILocator configuredLocator)
        {
            return new ReadOnlyLocator(configuredLocator);
        }

        public bool BuildUp(object target)
        {
            if (_ConfiguredLocator is ILocatorWithPropertyInjection propertyInjector)
            {
                return propertyInjector.BuildUp(target);
            }

            return false;
        }

        public ILocatorScoped CreateScope()
        {
            if (!(_ConfiguredLocator is ILocatorCreateScope scopedCreator))
            {
                throw new NullReferenceException($"{_ConfiguredLocator.GetType().FullName} doesn't support {typeof(ILocatorCreateScope).FullName}!");
            }

            return (_ConfiguredLocator as ILocatorCreateScope).CreateScope();
        }

        public void Dispose()
        {
            _ConfiguredLocator.Dispose();
        }

        public void EnsureLocked()
        {
            IsLocked = true;
        }

        public object Get(Type serviceType, string key = null)
        {
            return _ConfiguredLocator.Get(serviceType, key);
        }

        public T Get<T>(string key = null)
        {
            return _ConfiguredLocator.Get<T>(key);
        }

        public IEnumerable<T> GetAll<T>(string key = null)
        {
            return _ConfiguredLocator.GetAll<T>(key);
        }

        public IEnumerable<object> GetAll(Type serviceType, string key = null)
        {
            return _ConfiguredLocator.GetAll(serviceType, key);
        }
        
        private void ThrowIfLocked()
        {
            if (IsLocked)
            {
                throw new LocatorLockedException();
            }
        }
    }
}
