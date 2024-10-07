using Entities.DataTransferObjects;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Formatters;
using Microsoft.EntityFrameworkCore;
using Microsoft.Identity.Client;
using Presentation.ActionFilters;
using Repositories.Contracts;
using Repositories.EFCore;
using Services;
using Services.Contracts;

namespace WebApi.Extensions
{   
// This class contains extension methods for configuring services in the WebApi project.

    public static class ServicesExtensions
    {


        /**
         * Configures the SQL context for the service.
         * Adds the DbContext to the container with the specified SQL connection string.
         */
        public static void ConfigureSqlContext(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddDbContext<RepositoryContext>(options =>
                options.UseSqlServer(configuration.GetConnectionString("sqlConnection"))
            );

        }

        /**
         * Configures the repository manager for the service.
         * Adds the RepositoryManager to the container as a scoped service.
         */
        public static void ConfigureRepositoryManager(this IServiceCollection services) =>
             services.AddScoped<IRepositoryManager, RepositoryManager>();


        /**
         * Configures the service manager for the service.
         * Adds the IServiceManager to the container as a scoped service.
         */
        public static void ConfigureServiceManager(this IServiceCollection services) =>
            services.AddScoped<IServiceManager, ServiceManager>();



        // Configures the logger service for the service.
        // Adds the ILoggerService to the container as a singleton service.
        public static void ConfigureLoggerServive(this IServiceCollection servives) =>
            
            servives.AddSingleton<ILoggerService, LoggerManager>();


        /**
         * Configures the action filters for the service.
         * Adds the ValidationFilterAttribute and LogFilterAttribute to the container as scoped services.
         */
        public static void ConfigureActionFilters(this IServiceCollection services)
        {
            services.AddScoped<ValidationFilterAttribute>(); // IoC kaydı 
            services.AddSingleton<LogFilterAttribute>(); // IoC kaydı
            services.AddScoped<ValidateMediaTypeAttribute>(); // IoC kaydı
        }


        public static void ConfigureCors(this IServiceCollection services)
        {
            services.AddCors(options => { 
                
                options.AddPolicy("CorsPolicy", builder => builder
                    .AllowAnyOrigin()
                    .AllowAnyMethod()
                    .AllowAnyHeader()
                    .WithExposedHeaders("X-Pagination")
                );



            });
        }



        public static void ConfigureDataShaper(this IServiceCollection services)
        {
            services.AddScoped<IDataShaper<BookDto>, DataShaper<BookDto>>();
        }



        public static void AddCustomMediaTypes(this IServiceCollection services)
        {
            services.Configure<MvcOptions>(config =>
            {
                var systemTextJsonOutputFormatter = config
                .OutputFormatters
                .OfType<SystemTextJsonOutputFormatter>()?.FirstOrDefault();



                if (systemTextJsonOutputFormatter is not null)
                {
                    systemTextJsonOutputFormatter.SupportedMediaTypes.Add("application/vnd.ertuakkaya.hateoas+json");

                    systemTextJsonOutputFormatter.SupportedMediaTypes.Add("application/vnd.ertuakkaya.apiroot+json");
                }

                var xmlOutputFormatter = config.OutputFormatters.OfType<XmlDataContractSerializerOutputFormatter>()?.FirstOrDefault();


                if (xmlOutputFormatter is not null)
                {
                    xmlOutputFormatter.SupportedMediaTypes.Add("application/vnd.ertuakkaya.hateoas+xml");

                    xmlOutputFormatter.SupportedMediaTypes.Add("application/vnd.ertuakkaya.apiroot+xml");
                }

            });
        }

    }
}
