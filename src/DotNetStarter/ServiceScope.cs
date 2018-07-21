using System;

#if NETSTANDARD1_0 || NETSTANDARD1_1 || NETSTANDARD1_3 || NETSTANDARD2_0
using Microsoft.Extensions.DependencyInjection;
#endif

namespace DotNetStarter
{
#if NET45 || NET35 || NET40
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