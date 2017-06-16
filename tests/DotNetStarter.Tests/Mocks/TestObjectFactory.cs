using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using DotNetStarter.Abstractions;

//register this as the default configuration
[assembly: StartupObjectFactory(typeof(DotNetStarter.Tests.Mocks.TestObjectFactory))]

namespace DotNetStarter.Tests.Mocks
{
    internal class TestObjectFactory : StartupObjectFactory
    {
        public override int SortOrder => base.SortOrder + 100;

        public override IStartupLogger CreateStartupLogger() => new TestLogger();

        public override IStartupModuleFilter CreateModuleFilter() => new TestModuleFilter();

        public override ILocatorRegistry CreateRegistry(IStartupConfiguration config) => new TestLocator();

    }

    /// <summary>
    /// simple locator that only cares about startup modules
    /// </summary>
    internal class TestLocator : ILocatorRegistry
    {
        private List<Type> modules = new List<Type>();

        public object InternalContainer => null;

        public string DebugInfo => null;

        public void Add(Type serviceType, Type serviceImplementation, string key = null, LifeTime lifeTime = LifeTime.Transient, ConstructorType constructorType = ConstructorType.Greediest)
        {
            if (serviceType == typeof(IStartupModule))
                modules.Add(serviceImplementation);

        }

        public void Add(Type serviceType, Func<ILocator, object> implementationFactory, LifeTime lifeTime)
        {

        }

        public void Add(Type serviceType, object serviceInstance)
        {

        }

        public void Add<TService, TImpl>(string key = null, LifeTime lifetime = LifeTime.Transient, ConstructorType constructorType = ConstructorType.Greediest) where TImpl : TService
        {
            Add(typeof(TService), typeof(TImpl), key, lifetime, constructorType);
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
            return default(T);
        }

        public IEnumerable<T> GetAll<T>(string key = null)
        {
            if (typeof(T) == typeof(IStartupModule))
            {
                var startupModules =  modules.Select(x => Activator.CreateInstance(x)).OfType<T>();

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

    internal class TestModuleFilter : StartupModuleFilter
    {
        public override IEnumerable<IDependencyNode> FilterModules(IEnumerable<IDependencyNode> modules)
        {
            return modules.Where(x => !x.FullName.Contains(nameof(ExcludeModule))).OrderByDescending(x => x.DependencyCount);
        }
    }

    internal class TestAssemblyFilter : AssemblyFilter
    {
        public override bool FilterAssembly(Assembly assembly) => base.FilterAssembly(assembly);
    }

    internal class TestLogger : StringLogger { }

    [StartupModule]
    public class ExcludeModule : IStartupModule
    {
        public void Shutdown(IStartupEngine engine)
        {

        }

        public void Startup(IStartupEngine engine)
        {

        }
    }
}
