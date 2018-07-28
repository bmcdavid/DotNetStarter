using System;
using System.Collections.Generic;
using System.Linq;
using DotNetStarter.Abstractions;
using DotNetStarter.Internal;

namespace DotNetStarter.UnitTests.Mocks
{
    /// <summary>
    /// simple locator that only cares about startup modules
    /// </summary>
    internal class TestLocator : ILocatorRegistry, ILocator
    {
        private List<Type> modules = new List<Type>();

        private readonly Dictionary<Type, object> instances = new Dictionary<Type, object>();

        private IEnumerable<Type> allowedTypes = new Type[] { typeof(IStartupModule), typeof(ILocatorConfigure), typeof(IReflectionHelper) };

        public object InternalContainer => null;

        public string DebugInfo => null;

        public void Add(Type serviceType, Type serviceImplementation, string key = null, Lifecycle lifeTime = Lifecycle.Transient)
        {
            if (allowedTypes.Contains(serviceType))
                modules.Add(serviceImplementation);

        }

        public void Add(Type serviceType, Func<ILocator, object> implementationFactory, Lifecycle lifeTime)
        {

        }

        public void Add(Type serviceType, object serviceInstance)
        {
            instances[serviceType] = serviceInstance;
        }

        public void Add<TService, TImpl>(string key = null, Lifecycle lifetime = Lifecycle.Transient) where TImpl : TService
        {
            Add(typeof(TService), typeof(TImpl), key, lifetime);
        }

        public bool BuildUp(object target)
        {
            return true;
        }

        public bool ContainsService(Type serviceType, string key = null)
        {
            return false;
        }

        public void Dispose()
        {

        }

        public object Get(Type serviceType, string key = null)
        {
            return null;
        }

        public T Get<T>(string key = null)
        {
            if (typeof(T) == typeof(ILocatorAmbient)) { return new object[] { new LocatorAmbient(this) }.OfType<T>().Last(); }

            if (typeof(T) == typeof(IShutdownHandler))
            {
                IShutdownHandler x = new Internal.ShutdownHandler(instances[typeof(ILocator)] as ILocator, instances[typeof(IStartupConfiguration)] as IStartupConfiguration);

                return new object[] { x }.OfType<T>().Last();
            }

            if (allowedTypes.Contains(typeof(T)))
            {
                var startupModules = modules.Select(x => Activator.CreateInstance(x)).OfType<T>();

                return startupModules.Last();
            }

            return default(T);
        }

        public IEnumerable<T> GetAll<T>(string key = null)
        {
            if (allowedTypes.Contains(typeof(T)))
            {
                var startupModules = modules.Select(x => Activator.CreateInstance(x)).OfType<T>().ToList();

                instances[typeof(T)] = startupModules;

                return startupModules;
            }

            return null;
        }

        public IEnumerable<object> GetAll(Type serviceType, string key = null)
        {
            return null;
        }

        public ILocator OpenScope(object scopeName = null, object scopeContext = null)
        {
            return null;
        }

        public void Remove(Type serviceType, string key = null, Type serviceImplementation = null)
        {

        }
    }
}
