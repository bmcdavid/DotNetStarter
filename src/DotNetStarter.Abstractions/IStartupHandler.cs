using System;

namespace DotNetStarter.Abstractions
{
    /// <summary>
    /// Main hook for startup
    /// </summary>
    public interface IStartupHandler //: IStartupEngine
    {
        /// <summary>
        /// Creates the startup context
        /// </summary>
        /// <param name="startupConfiguration"></param>
        /// <returns></returns>
        IStartupContext ConfigureLocator(IStartupConfiguration startupConfiguration);

        /// <summary>
        /// Action to execute IStartupModules, returns true if modules executed
        /// </summary>
        bool TryExecuteStartupModules();
    }
}