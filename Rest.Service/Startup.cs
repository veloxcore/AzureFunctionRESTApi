using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Text;

namespace Rest.Service
{
    /// <summary>
    /// Register dependency and configure stuff
    /// </summary>
    public class Startup
    {
        /// <summary>
        /// Registers the dependency.
        /// </summary>
        /// <param name="services">The services.</param>
        /// <param name="configuration">The configuration.</param>
        public static void RegisterDependency(IServiceCollection services, IConfiguration configuration)
        {
            Rest.Data.Startup.RegisterDependency(services, configuration);

            // Register all services
            services.AddTransient<IBookService, BookService>();
            services.AddTransient<IMetaDataService, MetaDataService>();
        }
    }
}
