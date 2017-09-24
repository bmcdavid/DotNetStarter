namespace DotNetStarter.Abstractions
{
    using System;

    /// <summary>
    /// Assembly attribute to allow swapping of the default object factory. The higher sort number wins.
    /// </summary>
    [AttributeUsage(AttributeTargets.Assembly, AllowMultiple = true, Inherited = false)]
    public class StartupObjectFactoryAttribute : AssemblyFactoryBaseAttribute
    {
        private StartupObjectFactoryAttribute() : this(null) { }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="startupConfiguration"></param>
        public StartupObjectFactoryAttribute(Type startupConfiguration) : base(startupConfiguration, typeof(IStartupObjectFactory))
        {

        }
    }
}
