using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Repositories.EFCore;

namespace WebApi.ContextFactory
{
    public class RepositoryContextFactory : IDesignTimeDbContextFactory<RepositoryContext>
    {

        public RepositoryContext CreateDbContext(string[] args)
        {
            /**
             * configurationBuilder is used to build the configuration for the DbContext.
             */
            var configuration = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json")
                .Build();


            // DbContextOptionsBuilder is used to build the options for the DbContext.

            var builder = new DbContextOptionsBuilder<RepositoryContext>()
                .UseSqlServer(
                    configuration.GetConnectionString("sqlConnection"),
                    prj => prj.MigrationsAssembly("WebApi")
                );

            return new RepositoryContext(builder.Options);
        }


    }
}
