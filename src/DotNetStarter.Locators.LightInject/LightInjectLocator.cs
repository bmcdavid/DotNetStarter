using DotNetStarter.Abstractions;
using DotNetStarter.Abstractions.Internal;
using LightInject;
using System;
using System.Collections.Generic;
using System.Linq;

namespace DotNetStarter.Locators
{
    /// <summary>
    /// Default LightInject ILocatoryRegistry
    /// </summary>
    public class LightInjectLocator : ILocator, ILocatorWithCreateScope
    {
        private IServiceContainer _container;
        private ContainerRegistrationCollection _registrations;

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="serviceContainer"></param>
        /// <param name="containerRegistrations"></param>
        public LightInjectLocator(IServiceContainer serviceContainer, ContainerRegistrationCollection containerRegistrations)
        {
            _container = serviceContainer;
            _registrations = containerRegistrations;
        }

        /// <summary>
        /// Creates an ILocatorScoped
        /// </summary>
        /// <returns></returns>
        public ILocatorScoped CreateScope()
        {
            return new LightInjectLocatorScoped
            (
                _container.BeginScope(),
                this
            );
        }

        /// <summary>
        /// Disposes IServiceContainer
        /// </summary>
        public void Dispose()
        {
            // hack: disposing throws StackOverFlowException, so its not done
            //_container.Dispose();
        }

        /// <summary>
        /// Gets service type
        /// </summary>
        /// <param name="serviceType"></param>
        /// <param name="key"></param>
        /// <returns></returns>
        public object Get(Type serviceType, string key = null)
        {
            return _container.GetInstance(serviceType);
        }

        /// <summary>
        /// Gets T
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key"></param>
        /// <returns></returns>
        public T Get<T>(string key = null)
        {
            return _container.GetInstance<T>();
        }

        /// <summary>
        /// Gets all T
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key"></param>
        /// <returns></returns>
        public IEnumerable<T> GetAll<T>(string key = null)
        {
            var list = _container.GetAllInstances<T>();
            SortList(ref list, typeof(T));

            return list;
        }

        /// <summary>
        /// Gets all serviceType
        /// </summary>
        /// <param name="serviceType"></param>
        /// <param name="key"></param>
        /// <returns></returns>
        public IEnumerable<object> GetAll(Type serviceType, string key = null)
        {
            var list = _container.GetAllInstances(serviceType);
            SortList(ref list, serviceType);

            return list;
        }

        // hack: GetAllInstances Requires additional sorting as LightInject returns from ConcurrentDictionary
        // required for LightInject, which causes many other considerations as constructor injection isn't supported by this, affects DotNetStarter.Web injection
        /* IEnumerable will need another sorting mechanism for most things
         * I will need to work around it in IStartupModule cases like in DotNetStarter.Web
         * this also won't support delegate rolutions
         */
        private void SortList<T>(ref IEnumerable<T> list, Type service)
        {
            if (_registrations.TryGetValue(service, out List<ContainerRegistration> sourceList))
            {
                var typed = from o in sourceList.Select(x => x.ServiceImplementation)
                            join i in list
                            on o equals i.GetType()
                            select i;

                list = typed.ToList();
            };
        }
    }
}