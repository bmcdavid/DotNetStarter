using DotNetStarter.Abstractions;

namespace DotNetStarter.Extensions.WebApi
{
    /// <summary>
    /// Default is to disable DotNetStarter.Web's scope
    /// </summary>
    [Register(typeof(IPipelineScope), LifeTime.Singleton)]
    public class PipelineScope : IPipelineScope
    {
        /// <summary>
        /// Default is false
        /// </summary>
        public virtual bool Enabled => false;
    }
}