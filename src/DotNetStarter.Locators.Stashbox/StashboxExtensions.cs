using DotNetStarter.Abstractions;
using Stashbox.Lifetime;
using Stashbox.Registration;

namespace DotNetStarter.Locators
{
    internal static class StashboxExtensions
    {
        public static IFluentServiceRegistrator ConvertLifetime(this IFluentServiceRegistrator r, Lifecycle l)
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