using DotNetStarter.Abstractions;

namespace DotNetStarter.Extensions.WebApi
{
    /// <summary>
    /// Default registration values
    /// </summary>
    public class ApiControllerRegistrationSetup : IApiControllerRegistrationSetup
    {
        /// <summary>
        /// Default is to register api controllers
        /// </summary>
        public virtual bool EnableApiControllerRegistrations => true;
    }
}
