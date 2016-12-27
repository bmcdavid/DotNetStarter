namespace DotNetStarter
{
    using System.Collections.Generic;

    /// <summary>
    /// Imports all registered services
    /// </summary>
    /// <typeparam name="TService"></typeparam>
    public struct ImportAll<TService> where TService : class
    {
        private IEnumerable<TService> _Services;

        /// <summary>
        /// Imported services
        /// </summary>
        public IEnumerable<TService> Services
        {
            get
            {
                if (_Services == null) _Services = Context.Default.Locator.GetAll<TService>();

                return _Services;
            }
        }
    }
}
