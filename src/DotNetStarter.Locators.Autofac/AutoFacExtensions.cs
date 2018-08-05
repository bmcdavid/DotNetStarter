using Autofac.Builder;
using DotNetStarter.Abstractions;

namespace DotNetStarter.Locators
{
    internal static class AutoFacExtensions
    {
        internal static IRegistrationBuilder<object, TActivatorData, TRegistrationStyle> ConfigureLifecycle<TActivatorData, TRegistrationStyle>(
            this IRegistrationBuilder<object, TActivatorData, TRegistrationStyle> registrationBuilder,
            Lifecycle lifecycleKind,
            object lifetimeScopeTagForSingleton)
        {
            switch (lifecycleKind)
            {
                case Lifecycle.Singleton:
                    if (lifetimeScopeTagForSingleton == null)
                    {
                        registrationBuilder.SingleInstance();
                    }
                    else
                    {
                        registrationBuilder.InstancePerMatchingLifetimeScope(lifetimeScopeTagForSingleton);
                    }

                    break;

                case Lifecycle.Scoped:
                    registrationBuilder.InstancePerLifetimeScope();
                    break;

                case Lifecycle.Transient:
                    registrationBuilder.InstancePerDependency();
                    break;
            }

            return registrationBuilder;
        }
    }
}