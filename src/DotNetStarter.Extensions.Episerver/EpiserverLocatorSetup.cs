using DotNetStarter.Abstractions;
using EPiServer.Framework;
using EPiServer.Framework.Initialization;
using EPiServer.ServiceLocation;
using System;

#pragma warning disable CS0612 // Type or member is obsolete

// instructs DotNetStarter to use this to create ILocatorRegistry
[assembly: LocatorRegistryFactory(typeof(DotNetStarter.Extensions.Episerver.EpiserverLocatorSetup))]

namespace DotNetStarter.Extensions.Episerver
{
    /// <summary>
    /// Creates a DotNetStarter ILocatorFactory using Episerver's structuremap instance
    /// </summary>
    [Obsolete]
    [ModuleDependency]
    public class EpiserverLocatorSetup : IConfigurableModule, ILocatorRegistryFactory
    {
        static ILocatorRegistry _LocatorRegistry;

        /// <summary>
        /// Invoked to create a locator registry since Structuremap is now modular in Episerver. Use a System.Web.PreApplicationStartMethod to assign a startup func;
        /// </summary>
        [Obsolete]
        public static Func<ServiceConfigurationContext, ILocatorRegistry> CreateLocatorRegistry = null;

        /// <summary>
        /// Assign a custom action to invoke DotNetStarter startup process
        /// </summary>
        [Obsolete]
        public static Action InvokeDotNetStarter = null;

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="context"></param>
        public void ConfigureContainer(ServiceConfigurationContext context)
        {
            _LocatorRegistry = CreateLocatorRegistry?.Invoke(context);
            InvokeDotNetStarter?.Invoke();
        }

        /// <summary>
        /// Create locator registry
        /// </summary>
        /// <returns></returns>
        public ILocatorRegistry CreateRegistry()
        {
            if (_LocatorRegistry != null)
                return _LocatorRegistry;

            throw new Exception($"Please review https://bmcdavid.github.io/DotNetStarter/example-episerver-locator.html for setting up with Episerver.");
        }

        /// <summary>
        /// Intialize
        /// </summary>
        /// <param name="context"></param>
        public void Initialize(InitializationEngine context)
        {
            // ensure DotNetStarter has started
            context.InitComplete += (sender, args) =>
            {
                // ensures DotNetStarter has started
                var locator = ApplicationContext.Default.Locator;
            };
        }

        /// <summary>
        /// Uninitialize
        /// </summary>
        /// <param name="context"></param>
        public void Uninitialize(InitializationEngine context) { }
    }
}
