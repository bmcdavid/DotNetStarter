namespace DotNetStarter.Locators
{
    using DotNetStarter.Abstractions;
    using DryIoc;
    using System;
    using System.Collections.Generic;
    using System.Linq;

    /// <summary>
    /// Base DryIoc locator
    /// </summary>
    public abstract class DryIocLocatorBase : ILocator, ILocatorCreateScope, ILocatorWithPropertyInjection
    {
        /// <summary>
        /// Raw container reference
        /// </summary>
        protected IContainer _Container;

        /// <summary>
        /// Constructor
        /// </summary>
        public DryIocLocatorBase(IContainer container = null)
        {
            var rules = Rules.Default
                .WithoutThrowIfDependencyHasShorterReuseLifespan()
                .WithFactorySelector(Rules.SelectLastRegisteredFactory())
                .WithTrackingDisposableTransients() //used in transient delegate cases
                .WithImplicitRootOpenScope()
                ;

            _Container = container ?? new Container(rules);
        }

        /// <summary>
        /// Provides debug information about the container
        /// </summary>
        public string DebugInfo => _Container?.GetServiceRegistrations()?.Select(x => x.ToString()).
            Aggregate((current, next) => current + Environment.NewLine + next);

        /// <summary>
        /// Allows access to wrapped container
        /// </summary>
        public virtual object InternalContainer => _Container;

        /// <summary>
        /// Builds up properties of given object, useful in webforms.
        /// </summary>
        /// <param name="target"></param>
        /// <returns></returns>
        public virtual bool BuildUp(object target)
        {
            try
            {
                _Container.InjectPropertiesAndFields(target); // v2.x

                return true;
            }
            catch (Exception e)
            {
                if (e is ContainerException ce)
                    throw new StartupContainerException(ce.Error, ce.Message, ce.InnerException);

                throw new StartupContainerException(-100, e.Message, e.InnerException);
            }
        }

        /// <summary>
        /// Dispose
        /// </summary>
        public virtual void Dispose()
        {
            _Container?.Dispose();
        }

        /// <summary>
        /// Gets service instance given type and optional key
        /// </summary>
        /// <param name="service"></param>
        /// <param name="key"></param>
        /// <returns></returns>
        public virtual object Get(Type service, string key = null) => _Container.Resolve(service, key, IfUnresolved.Throw);

        /// <summary>
        /// Gets service instance given type and optional key for generics
        /// </summary>
        /// <typeparam name="TService"></typeparam>
        /// <param name="key"></param>
        /// <returns></returns>
        public virtual TService Get<TService>(string key = null) => (TService)Get(typeof(TService), key);

        /// <summary>
        /// Gets all registered services. Key is optional.
        /// </summary>
        /// <param name="serviceType"></param>
        /// <param name="key"></param>
        /// <returns></returns>
        public virtual IEnumerable<object> GetAll(Type serviceType, string key = null) =>
                    _Container.ResolveMany(serviceType, serviceKey: key); //_Container.Resolve(typeof(IEnumerable<>).MakeGenericType(serviceType), IfUnresolved.ReturnDefault) as IEnumerable<object>;

        /// <summary>
        /// Gets all registered services for generics.
        /// </summary>
        /// <typeparam name="TService"></typeparam>
        /// <param name="key"></param>
        /// <returns></returns>
        public virtual IEnumerable<TService> GetAll<TService>(string key = null) =>
                    _Container.ResolveMany<TService>(serviceKey: key); //_Container.Resolve<IEnumerable<TService>>(IfUnresolved.ReturnDefault);


        /// <summary>
        /// Creates/opens locator scope
        /// </summary>
        /// <returns></returns>
        public virtual ILocatorScoped CreateScope()
        {
            return new DryIocLocatorScoped(
                _Container
                //.CreateFacade() // allows registrations to only exist in the instance
                .OpenScope(),
                this
            );
        }
    }
}