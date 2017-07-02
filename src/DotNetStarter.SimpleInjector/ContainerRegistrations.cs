namespace DotNetStarter
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    internal class ContainerRegistrations : Dictionary<Type, HashSet<ContainerRegistration>>
    {
        public string DebugInformation()
        {
            return string.Join(",", this.Select(x => x.Key));//todo, build better output
        }
    }
}