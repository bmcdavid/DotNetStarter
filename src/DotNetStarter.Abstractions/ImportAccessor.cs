﻿using System.Collections.Generic;

namespace DotNetStarter.Abstractions
{
    /// <summary>
    /// The import accessor allows for Imports to have services assigned bypassing the locator.
    /// </summary>
    /// <typeparam name="TService"></typeparam>
    public class ImportAccessor<TService> where TService : class
    {
        /// <summary>
        /// TService instance
        /// </summary>
        public TService Service;

        /// <summary>
        /// TService sequence
        /// </summary>
        public IEnumerable<TService> AllServices;

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="service"></param>
        /// <param name="allServices"></param>
        public ImportAccessor(TService service, IEnumerable<TService> allServices)
        {
            Service = service ?? throw new System.ArgumentNullException(nameof(service));
            AllServices = allServices ?? throw new System.ArgumentNullException(nameof(allServices));
        }
    }
}