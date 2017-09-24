using DotNetStarter.Abstractions;

namespace DotNetStarter.Extensions.WebApi
{
    /// <summary>
    /// Default registration values
    /// </summary>
    public class ApiControllerRegistrationSetup : IApiControllerRegistrationSetup
    {
        /// <summary>
        /// Default lifetime is scoped
        /// </summary>
        public virtual LifeTime ApiControllerLifeTime => LifeTime.Scoped;

        /// <summary>
        /// Default is to register api controllers
        /// </summary>
        public virtual bool EnableApiControllerRegistrations => true;
    }
}
