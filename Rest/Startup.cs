using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Hosting;
using Microsoft.Azure.WebJobs.Logging;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Rest.Models;
using Rest.Data;
using Rest.Model.DTO;
using Rest.Model.Entity;
using Rest.Data.Infrastructure;
using Rest.Data.Repository;
using System;
using Willezone.Azure.WebJobs.Extensions.DependencyInjection;
using Rest.Service;
using FluentValidation;
using FluentValidation.AspNetCore;
using Rest.Models.Validators;
using System.Linq;
using Serilog.Sinks.AzureWebJobsTraceWriter;
using Microsoft.ApplicationInsights;
using Microsoft.ApplicationInsights.Extensibility;
using Microsoft.WindowsAzure.Storage;

[assembly: WebJobsStartup(typeof(Rest.Startup))]
namespace Rest
{
    /// <summary>
    /// Startup class
    /// </summary>
    internal class Startup : IWebJobsStartup
    {
        /// <summary>
        /// Configure Builder
        /// </summary>
        /// <param name="Builder">Builder</param>
        public void Configure(IWebJobsBuilder builder)
        {
            builder.AddDependencyInjection<ServiceProviderBuilder>();
        }

        /// <summary>
        /// Service provider builder
        /// </summary>
        internal class ServiceProviderBuilder : IServiceProviderBuilder
        {
            private readonly ILoggerFactory _loggerFactory;

            /// <summary>
            /// Constructor
            /// </summary>
            /// <param name="loggerFactory">LoggerFactory</param>
            public ServiceProviderBuilder(ILoggerFactory loggerFactory)
            {
                _loggerFactory = loggerFactory;
            }

            /// <summary>
            /// Build Service and inject dependancy
            /// </summary>
            /// <returns>ServiceProvider</returns>
            public IServiceProvider Build()
            {
                ServiceCollection services = new ServiceCollection();

                IConfigurationRoot config = new ConfigurationBuilder()
                    .SetBasePath(Environment.CurrentDirectory)
                    .AddJsonFile("local.settings.json", optional: true, reloadOnChange: true)
                    .AddEnvironmentVariables()
                    .Build();

                ///Register Dependency of services
                Service.Startup.RegisterDependency(services, config);

                // Important: We need to call CreateFunctionUserCategory, otherwise our log entries might be filtered out.
                services.AddSingleton<ILogger>(_ => _loggerFactory.CreateLogger(LogCategories.CreateFunctionUserCategory("Common")));

                // Configure AutoMapper
                AutoMapper.Mapper.Initialize(cfg =>
                {
                    cfg.CreateMap<Book, BookDTO>()
                        .ForMember(o => o.Authors, o => o.MapFrom((a, b) => a.Authors.Split(',')));
                    cfg.CreateMap<BookModel, Book>()
                        .ForMember(o => o.Authors, o => o.MapFrom(a => string.Join(",", a.Authors)))
                        .ForMember(o => o.BookId, o => o.MapFrom(a => Guid.NewGuid().ToString().ToUpper()))
                        .ForMember(o => o.CreatedDate, o => o.Ignore())
                        .ForMember(o => o.UpdatedDate, o => o.Ignore())
                        .ReverseMap()
                        .ForMember(o => o.Authors, o => o.MapFrom((a, b) => a.Authors.Split(',')));
                });

                //FulentValidation
                services.AddMvc().AddFluentValidation();
                services.AddTransient<IValidator<Book>, BookValidator>();

                //Application Insights Telemetry
                string appInsights_InstrumentationKey = config.GetValue<string>("APPINSIGHTS_INSTRUMENTATIONKEY");
                if (appInsights_InstrumentationKey != null)
                    LoggerExtensions.telemetry.InstrumentationKey = appInsights_InstrumentationKey;
             
                //Register cloudstorageAccount with connection from configuration
                string azureWebJobsStorageConnection = config.GetValue<string>("AzureWebJobsStorage");
                services.AddScoped(_ => CloudStorageAccount.Parse(azureWebJobsStorageConnection));

                ///Set Metadata payload Keys
                string payloadKeys = config.GetValue<string>("Headers");
                if (!string.IsNullOrEmpty(payloadKeys))
                    HttpRequestExtensions.PayLoadKeys = payloadKeys.Split(',').ToList();
                return services.BuildServiceProvider(true);
            }
        }
    }

}
