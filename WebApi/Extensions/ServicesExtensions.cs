using Microsoft.EntityFrameworkCore;
using Microsoft.Identity.Client;
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
    }
}
