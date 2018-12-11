using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Rest.Data.Infrastructure;
using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.EntityFrameworkCore;
using Rest.Data.Repository;

namespace Rest.Data
{
    /// <summary>
    /// Startup for Data To Register Depenedencies of Rest.Data
    /// </summary>
    public class Startup
    {
        /// <summary>
        /// Register Dependency of data 
        /// </summary>
        /// <param name="services">Service Collection</param>
        /// <param name="configuration">Configuration</param>
        public static void RegisterDependency(IServiceCollection services, IConfiguration configuration)
        {
            ///Get Connection Sting For Configration
            string connectionString = configuration.GetConnectionString("SqlConnectionString");
            //services.AddDbContext<AppDbContext>(options => options.UseSqlServer(connectionString));

            //// Infrastructure
            services.AddTransient<IDbQueryProcessor, DbQueryProcessor>();
            services.AddTransient(typeof(IDbConnectionHelper), (s) => new DbConnectionHelper(connectionString));
            services.AddTransient<ITransactionHelper, TransactionHelper>();

            //Repository
            services.AddTransient<IBookRepository, BookRepository>();
            services.AddTransient<IMetaDataRepository, MetaDataRepository>();
        }
    }
}
