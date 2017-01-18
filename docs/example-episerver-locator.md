---
title: DotNetStarter - Episerver locator and Depencency Resolver
---
# DotNetStarter - Episerver locator and Depencency Resolver

Below is an example of how to create an ILocator with Episerver's structuremap configured container.

### Required Nuget packages

* DotNetStarter
* DotNetStarter.Web
* EPiServer.CMS

```cs
using DotNetStarter.Abstractions;
using DotNetStarter.Internal;
using DotNetStarter.Web;
using EPiServer.Framework;
using EPiServer.Framework.Initialization;
using EPiServer.ServiceLocation;
using StructureMap;
using StructureMap.Pipeline;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

// instructs DotNetStarter to use this to create ILocatorRegistry
[assembly: LocatorRegistryFactory(typeof(Example.EpiserverLocatorSetup))]

namespace Example
{
    // set dependency on what normally sets the MVC Dependency Resolver
    [ModuleDependency]
    public class EpiserverLocatorSetup : IConfigurableModule, ILocatorRegistryFactory
    {
        static IContainer _Container; // must be static to share between instances

        public void ConfigureContainer(ServiceConfigurationContext context)
        {
            _Container = context.Container; // store the containr for use in CreateRegistry

            // creates a new dependency resolver via the Locator instead of typical context.Container
            DependencyResolver.SetResolver(new EpiserverLocatorDependencyResolver(DotNetStarter.Context.Default.Locator));
        }

        public ILocatorRegistry CreateRegistry() => new EpiserverStructuremapLocator(_Container);

        public void Initialize(InitializationEngine context) { }

        public void Uninitialize(InitializationEngine context) { }
    }

    /// <summary>
    /// Requires DotNetStarter.Web to create a scoped locator in the HttpContext.Items
    /// </summary>
    public class EpiserverLocatorDependencyResolver : IDependencyResolver
    {
        private ILocator Locator;

        public EpiserverLocatorDependencyResolver(ILocator locator)
        {
            Locator = locator;
        }

        public object GetService(Type serviceType)
        {
            var temp = HttpContext.Current?.GetScopedLocator() ?? Locator;

            return temp.Get(serviceType);
        }

        public IEnumerable<object> GetServices(Type serviceType)
        {
            var temp = HttpContext.Current?.GetScopedLocator() ?? Locator;

            return temp.GetAll(serviceType);
        }
    }

    /// <summary>
    /// Locator wrapping Episerver _Container
    /// </summary>
    public class EpiserverStructuremapLocator : ILocatorRegistry
    {
        IContainer _Container;

        public EpiserverStructuremapLocator(IContainer container)
        {
            _Container = container;
        }

        public string DebugInfo => _Container.WhatDoIHave();

        public object InternalContainer => _Container;

        private ILifecycle ConvertLifeTime(LifeTime lifetime)
        {
            switch (lifetime)
            {
                case LifeTime.Transient:
                    return Lifecycles.Transient;
                case LifeTime.Singleton:
                    return Lifecycles.Singleton;
                case LifeTime.HttpRequest:
                case LifeTime.Scoped:
                    return Lifecycles.Container;
                case LifeTime.AlwaysUnique:
                    return Lifecycles.Unique;

            }

            return Lifecycles.Transient;
        }

        public void Add(Type serviceType, object serviceInstance)
        {
            _Container.Configure(x => x.For(serviceType).Singleton().Use(serviceInstance));
        }

        public void Add(Type serviceType, Func<ILocator, object> implementationFactory, LifeTime lifeTime)
        {
            _Container.Configure(x => x.For(serviceType).LifecycleIs(ConvertLifeTime(lifeTime)).Use((context) => implementationFactory.Invoke(context.GetInstance<ILocator>())));
        }

        public void Add(Type serviceType, Type serviceImplementation, string key = null, LifeTime lifeTime = LifeTime.Transient, ConstructorType constructorType = ConstructorType.Greediest)
        {
            _Container.Configure(x => x.For(serviceType).LifecycleIs(ConvertLifeTime(lifeTime)).Use(serviceImplementation));
        }

        public void Add<TService, TImpl>(string key = null, LifeTime lifetime = LifeTime.Transient, ConstructorType constructorType = ConstructorType.Greediest) where TImpl : TService
        {
            Add(typeof(TService), typeof(TImpl), key, lifetime, ConstructorType.Empty);
        }

        public bool BuildUp(object target)
        {
            _Container.BuildUp(target);

            return true;
        }

        public bool ContainsService(Type serviceType, string key = null)
        {
            if (key == null)
                return _Container.TryGetInstance(serviceType) != null;

            return _Container.TryGetInstance(serviceType, key) != null;
        }

        public void Dispose()
        {
            _Container?.Dispose();
        }

        public object Get(Type serviceType, string key = null)
        { 
            try
            {
                return _Container.GetInstance(serviceType);
            }
            catch { return null; }
        }

        public T Get<T>(string key = null)
        {
            try
            {
                return _Container.GetInstance<T>();
            }
            catch { return default(T); }
        }

        public IEnumerable<object> GetAll(Type serviceType, string key = null)
        {
            return _Container.GetAllInstances(serviceType).OfType<object>();
        }

        public IEnumerable<T> GetAll<T>(string key = null)
        {
            return _Container.GetAllInstances<T>();
        }

        public ILocator OpenScope(object scopeName = null, object scopeContext = null)
        {
            return new EpiserverStructuremapLocator(_Container.CreateChildContainer());
        }

        public void Remove(Type serviceType, string key = null, Type serviceImplementation = null)
        {
            if (serviceImplementation == null)
            {
                _Container.Model.EjectAndRemove(serviceType);
            }
            else
            {
                _Container.Model.EjectAndRemoveTypes((type) =>
                {
                    if (type != serviceType)
                        return false;

                    return serviceImplementation.IsAssignableFromCheck(type);
                });
            }
        }
    }
}
```