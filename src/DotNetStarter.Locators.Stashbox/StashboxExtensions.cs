using DotNetStarter.Abstractions;
using Stashbox.Lifetime;
using Stashbox.Registration.Fluent;

namespace DotNetStarter.Locators
{
    internal static class StashboxExtensions
    {
        public static FluentServiceConfigurator<RegistrationConfigurator> ConvertLifetime(this FluentServiceConfigurator<RegistrationConfigurator> r, Lifecycle l)
        {
            switch (l)
            {
                case Lifecycle.Scoped: return r.WithLifetime(new ScopedLifetime());
                case Lifecycle.Singleton: return r.WithLifetime(new SingletonLifetime());
                case Lifecycle.Transient:
                default:
                    return r;
            }
        }
    }
}