namespace DotNetStarter
{
    using Abstractions;
    using System.Reflection;

    /// <summary>
    /// Default Assembly filter
    /// </summary>
    public class AssemblyFilter : IAssemblyFilter
    {
        /// <summary>
        /// Default filters mscorelib, system, microsoft
        /// </summary>
        /// <param name="assembly"></param>
        /// <returns></returns>
        public virtual bool FilterAssembly(Assembly assembly)
        {
#if NETFULLFRAMEWORK
            if (assembly.GlobalAssemblyCache) return true;
#endif

#if !NET35
            if (assembly.IsDynamic) return true;
#endif

            var name = assembly.GetName().Name.Split('.');

            switch (name[0].ToLower())
            {
                case "mscorlib":
                case "system":
                case "microsoft":
                    return true;
            }

            return false;
        }
    }
}