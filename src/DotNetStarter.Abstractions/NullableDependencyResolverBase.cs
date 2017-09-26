using System;
using System.Collections.Generic;
using System.Linq;

namespace DotNetStarter.Abstractions
{
    /// <summary>
    /// Base class for nullable dependency resolvers
    /// </summary>
    public abstract class NullableDependencyResolverBase
    {
        /// <summary>
        /// Types that can return null for GetService
        /// </summary>
        protected IEnumerable<Type> _AllowableNullForGetService;

        /// <summary>
        /// Types that can return null for GetServices
        /// </summary>
        protected IEnumerable<Type> _AllowableNullForGetServices;

        /// <summary>
        /// ILocator instance
        /// </summary>
        protected ILocator _Locator;

        /// <summary>
        /// Reflection helper instance
        /// </summary>
        protected IReflectionHelper _ReflectionHelper;

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="locator"></param>
        /// <param name="reflectionHelper"></param>
        /// <param name="allowableNullForGetService"></param>
        /// <param name="allowableNullForGetServices"></param>
        public NullableDependencyResolverBase(ILocator locator, IReflectionHelper reflectionHelper = null, IEnumerable<Type> allowableNullForGetService = null, IEnumerable<Type> allowableNullForGetServices = null)
        {
            _Locator = locator;
            _ReflectionHelper = reflectionHelper ?? _Locator.Get<IReflectionHelper>();
            _AllowableNullForGetService = allowableNullForGetService;
            _AllowableNullForGetServices = allowableNullForGetServices;
        }

        /// <summary>
        /// Default allowable null services for GetService calls
        /// </summary>
        /// <returns></returns>
        protected abstract IEnumerable<Type> DefaultAllowableNullForGetService();

        /// <summary>
        /// Default allowable null services for GetServices calls
        /// </summary>
        /// <returns></returns>
        protected abstract IEnumerable<Type> DefaultAllowableNullForGetServices();

        /// <summary>
        /// Resolves which locator to use, for instance a scoped instance
        /// </summary>
        /// <returns></returns>
        protected abstract ILocator ResolveLocator();

        /// <summary>
        /// Try/catch wrapper, will throw exceptions if locator is configured to, and requested service is not registered.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="builder"></param>
        /// <param name="allowedNulls"></param>
        /// <param name="serviceType"></param>
        /// <param name="errorMessage"></param>
        /// <param name="defaultValue"></param>
        /// <returns></returns>
        protected virtual T TryGet<T>(Func<T> builder, IEnumerable<Type> allowedNulls, Type serviceType, string errorMessage, T defaultValue)
        {
            try
            {
                return builder();
            }
            catch (Exception e)
            {
                if (allowedNulls.Any(x => _ReflectionHelper.IsAssignableFrom(x, serviceType)))
                {
                    return defaultValue;
                }

                throw new Exception(errorMessage, e);
            }
        }

        /// <summary>
        /// Tries to get service but only returns null for allowable null services.
        /// </summary>
        /// <param name="serviceType"></param>
        /// <returns></returns>
        protected virtual object TryGetService(Type serviceType)
        {
            EnsureAllowableNulls();

            return TryGet
            (
                () => ResolveLocator().Get(serviceType),
                _AllowableNullForGetService,
                serviceType,
                "Cannot create a service for " + serviceType.FullName,
                null
            );
        }

        /// <summary>
        /// Tries to get service but only returns null for allowable null services.
        /// </summary>
        /// <param name="serviceType"></param>
        /// <returns></returns>
        protected virtual IEnumerable<object> TryGetServices(Type serviceType)
        {
            EnsureAllowableNulls();

            return TryGet
            (
                () => ResolveLocator().GetAll(serviceType),
                _AllowableNullForGetServices,
                serviceType,
                "Cannot create services for " + serviceType.FullName,
                Enumerable.Empty<object>()
            );
        }

        /// <summary>
        /// Cannot do in constructor as MVC requires dependencies in their default methods
        /// </summary>
        private void EnsureAllowableNulls()
        {
            _AllowableNullForGetService = _AllowableNullForGetService ?? DefaultAllowableNullForGetService();
            _AllowableNullForGetServices = _AllowableNullForGetServices ?? DefaultAllowableNullForGetServices();
        }
    }
}
