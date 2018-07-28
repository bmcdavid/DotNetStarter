using System;

#if NETSTANDARD
using Microsoft.Extensions.DependencyInjection;
#endif

namespace DotNetStarter
{
#if NETFULLFRAMEWORK && !NETSTANDARD
    /// <summary>
    /// Access to scoped provider
    /// </summary>
    public interface IServiceScope : IDisposable
    {
        /// <summary>
        /// Scoped service provider
        /// </summary>
        IServiceProvider ServiceProvider { get; }
    }
#endif

    /// <summary>
    /// Access to scoped service provider
    /// </summary>
    public class ServiceScope : IServiceScope
    {
        private readonly IServiceProvider _provider;

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="provider"></param>
        public ServiceScope(IServiceProvider provider)
        {
            _provider = provider;
        }

        /// <summary>
        /// Scoped IServiceProvider
        /// </summary>
        public IServiceProvider ServiceProvider => _provider;

        /// <summary>
        /// Dispose service provider
        /// </summary>
        public void Dispose()
        {
            (_provider as IDisposable)?.Dispose();
        }
    }
}