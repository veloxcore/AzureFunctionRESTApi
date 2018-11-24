using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Hosting;
using Microsoft.Azure.WebJobs.Logging;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Rest.Models;
using Rest.Data;
using Rest.Data.DTO;
using Rest.Data.Entity;
using Rest.Data.Infrastructure;
using Rest.Data.Repository;
using System;
using Willezone.Azure.WebJobs.Extensions.DependencyInjection;

[assembly: WebJobsStartup(typeof(Rest.Startup))]
namespace Rest
{
    internal class Startup : IWebJobsStartup
    {
        public void Configure(IWebJobsBuilder builder)
        {
            builder.AddDependencyInjection<ServiceProviderBuilder>();
        }

        internal class ServiceProviderBuilder : IServiceProviderBuilder
        {
            private readonly ILoggerFactory _loggerFactory;

            public ServiceProviderBuilder(ILoggerFactory loggerFactory)
            {
                _loggerFactory = loggerFactory;
            }

            public IServiceProvider Build()
            {
                ServiceCollection services = new ServiceCollection();

                IConfigurationRoot config = new ConfigurationBuilder()
                    .SetBasePath(Environment.CurrentDirectory)
                    .AddJsonFile("local.settings.json", optional: true, reloadOnChange: true)
                    .AddEnvironmentVariables()
                    .Build();

                string connectionString = config.GetConnectionString("SqlConnectionString");
                services.AddDbContext<AppDbContext>(options => options.UseSqlServer(connectionString));

                // Repositories n Data
                services.AddTransient<IUnitOfWork, UnitOfWork>();
                services.AddTransient<IBookRepository, BookRepository>();

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

                return services.BuildServiceProvider(true);
            }
        }
    }

}
