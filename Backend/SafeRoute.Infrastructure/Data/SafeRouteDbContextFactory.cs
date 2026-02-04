using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;
using SafeRoute.Infrastructure.Data;
using System.IO;

namespace SafeRoute.Infrastructure.Data
{
    public class SafeRouteDbContextFactory
        : IDesignTimeDbContextFactory<SafeRouteDbContext>
    {
        public SafeRouteDbContext CreateDbContext(string[] args)
        {
            var configuration = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json")
                .Build();

            var optionsBuilder = new DbContextOptionsBuilder<SafeRouteDbContext>();

            optionsBuilder.UseSqlServer(
                configuration.GetConnectionString("DefaultConnection")
            );

            return new SafeRouteDbContext(optionsBuilder.Options);
        }
    }
}
