using System;
using DotNetStarter.Abstractions;

#if NETSTANDARD1_0 || NETSTANDARD1_1 || NETSTANDARD2_0
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
    [Register(typeof(IServiceScope), LifeTime.Scoped)]
    public class ServiceScope : IServiceScope
    {
        IServiceProvider Provider;

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="provider"></param>
        public ServiceScope(IServiceProvider provider)
        {
            Provider = provider;
        }

        /// <summary>
        /// Scoped IServiceProvider
        /// </summary>
        public IServiceProvider ServiceProvider => Provider;

        /// <summary>
        /// Dispose service provider
        /// </summary>
        public void Dispose()
        {
            (Provider as IDisposable)?.Dispose();
        }
    }
}