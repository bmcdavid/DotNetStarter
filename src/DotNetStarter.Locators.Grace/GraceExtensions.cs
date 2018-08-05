using DotNetStarter.Abstractions;
using Grace.DependencyInjection;

namespace DotNetStarter.Locators
{
    internal static class GraceExtensions
    {
        internal static IFluentExportStrategyConfiguration ConfigureLifetime(this IFluentExportStrategyConfiguration configuration, Lifecycle lifetime)
        {
            switch (lifetime)
            {
                case Lifecycle.Scoped:
                    return configuration.Lifestyle.SingletonPerScope();

                case Lifecycle.Singleton:
                    return configuration.Lifestyle.Singleton();

                default:
                    return configuration;
            }
        }

        internal static IFluentExportInstanceConfiguration<T> ConfigureLifetime<T>(this IFluentExportInstanceConfiguration<T> configuration, Lifecycle lifecycleKind)
        {
            switch (lifecycleKind)
            {
                case Lifecycle.Scoped:
                    return configuration.Lifestyle.SingletonPerScope();

                case Lifecycle.Singleton:
                    return configuration.Lifestyle.Singleton();
            }

            return configuration;
        }
    }
}