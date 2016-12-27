namespace DotNetStarter
{
    /// <summary>
    /// Imports a registered service
    /// </summary>
    /// <typeparam name="TService"></typeparam>
    public struct Import<TService> where TService : class
    {
        private TService _Service;                

        /// <summary>
        /// Imported service
        /// </summary>
        public TService Service
        {
            get
            {
                if (_Service == null)
                    _Service = Context.Default.Locator.Get<TService>();

                return _Service;
            }
        }
    }
}
