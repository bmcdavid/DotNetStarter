using DotNetStarter.Abstractions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace DotNetStarter
{
    /// <summary>
    /// Default IServiceProviderTypeChecker
    /// </summary>
    [Registration(typeof(IServiceProviderTypeChecker), Lifecycle.Singleton)]
    public class ServiceProviderTypeChecker : IServiceProviderTypeChecker
    {
        private readonly IStartupConfiguration _StartupConfiguration;
        private readonly IReflectionHelper _ReflectionHelper;
        Dictionary<Assembly, bool> _ScannedLookups;

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="startupConfiguration"></param>
        /// <param name="reflectionHelper"></param>
        public ServiceProviderTypeChecker(IStartupConfiguration startupConfiguration, IReflectionHelper reflectionHelper)
        {
            _StartupConfiguration = startupConfiguration;
            _ReflectionHelper = reflectionHelper;
            _ScannedLookups = new Dictionary<Assembly, bool>();
        }

        /// <summary>
        /// If the service type was in a scanned assembly, throw the exception
        /// </summary>
        /// <param name="serviceType"></param>
        /// <param name="exception"></param>
        /// <returns></returns>
        public virtual bool IsScannedAssembly(Type serviceType, Exception exception)
        {
            // if the configuration does not have a null assembly filter, we cannot rely on the assembly sequence as it could be all application assemblies
            if (_StartupConfiguration.AssemblyFilter != null)
                return false;

            var assembly = _ReflectionHelper.GetAssembly(serviceType);

            if (!_ScannedLookups.TryGetValue(assembly, out bool isScanned))
            {
                isScanned = _StartupConfiguration.Assemblies.Any(x => x == assembly);
                _ScannedLookups[assembly] = isScanned;
            }

            if (exception != null && isScanned == false)
            {
                LogException(serviceType, exception);
            }

            return isScanned;
        }

        /// <summary>
        /// Logs a warning to startup logger if assembly filter is not null and LogLevel threshold is set to Debug
        /// </summary>
        /// <param name="serviceType"></param>
        /// <param name="e"></param>
        protected virtual void LogException(Type serviceType, Exception e)
        {
            if (_StartupConfiguration.AssemblyFilter != null)
            {
                _StartupConfiguration.Logger.LogException($"Unable to resolve {serviceType.FullName}!", e, typeof(ServiceProviderTypeChecker), LogLevel.Debug);
            }
        }
    }
}
