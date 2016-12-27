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
        /// Default filter excludes: mscorelib, system, microsoft, ektron, and episerver namespaces
        /// </summary>
        /// <param name="assembly"></param>
        /// <returns></returns>
        public virtual bool FilterAssembly(Assembly assembly)
        {
            var name = assembly.GetName().Name.Split('.');

            switch (name[0].ToLower())
            {
                case "mscorlib":
                case "system":
                case "microsoft":
                case "ektron":
                case "episerver":
                    return false;
            }

            return true;
        }
    }
}