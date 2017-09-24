#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member

using DotNetStarter.Abstractions;
using DotNetStarter.Abstractions.Internal;
using System;
using System.Collections.Generic;

namespace DotNetStarter.Internal
{
    public sealed class ReadOnlyLocator : IReadOnlyLocator, ILocatorSetContainer, ILocatorCreateScope
    {
        static bool _IsLocked = false;
        ILocator _ConfiguredLocator;

        public ReadOnlyLocator(ILocator configuredLocator)
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

        public bool IsLocked => _IsLocked;

        public bool BuildUp(object target)
        {
            return _ConfiguredLocator.BuildUp(target);
        }

        public ILocatorScoped CreateScope(IScopeKind scopeKind)
        {
            var scopedCreator = _ConfiguredLocator as ILocatorCreateScope;

            if (scopedCreator == null)
                throw new NullReferenceException($"{_ConfiguredLocator.GetType().FullName} doesn't support {typeof(ILocatorCreateScope).FullName}!");

            return (_ConfiguredLocator as ILocatorCreateScope).CreateScope(scopeKind);
        }

        public void Dispose()
        {
            _ConfiguredLocator.Dispose();
        }

        public void EnsureLocked()
        {
            _IsLocked = true;
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

        public ILocator OpenScope(object scopeName = null, object scopeContext = null)
        {
            return _ConfiguredLocator.OpenScope(scopeName, scopeContext);
        }

        public void SetContainer(object container)
        {
            ThrowIfLocked();
            EnsureLocked();
            (_ConfiguredLocator as ILocatorSetContainer)?.SetContainer(container);
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
