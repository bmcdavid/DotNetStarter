using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DotNetStarter.Abstractions
{
    /// <summary>
    /// Null Startup Handler during startup
    /// </summary>
    public class NullStartupHandlerException : Exception
    {
        /// <summary>
        /// Constructor
        /// </summary>
        public NullStartupHandlerException() :
            base($"No {typeof(IStartupHandler).FullName} was defined during startup!")
        {
        }
    }
}
