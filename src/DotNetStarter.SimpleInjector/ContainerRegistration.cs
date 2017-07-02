namespace DotNetStarter
{
    using DotNetStarter.Abstractions;
    using System;

    internal class ContainerRegistration
    {
        public Type ServiceType { get; set; }

        public Type ServiceImplementation { get; set; }

        public object ServiceInstance { get; set; }

        public LifeTime LifeTime { get; set; }

        public Func<ILocator, object> ServiceFactory { get; set; }
    }
}