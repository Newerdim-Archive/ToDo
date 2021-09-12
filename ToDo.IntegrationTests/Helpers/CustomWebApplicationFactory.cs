using System.Diagnostics.CodeAnalysis;
using System.Linq;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using ToDo.API.Data;

namespace ToDo.IntegrationTests.Helpers
{
    /// <summary>
    /// Custom WebApplicationFactory with in memory db
    /// </summary>
    /// <typeparam name="TStartup"></typeparam>
    [ExcludeFromCodeCoverage]
    public class CustomWebApplicationFactory<TStartup> :
        WebApplicationFactory<TStartup>
            where TStartup : class
    {
        protected override void ConfigureWebHost(IWebHostBuilder builder)
        {
            builder.ConfigureServices(services =>
            {
                var descriptor = services
                    .SingleOrDefault(d => d.ServiceType == typeof(DbContextOptions<DataContext>));

                if (descriptor != null)
                {
                    services.Remove(descriptor);
                }

                services.AddDbContext<DataContext>(options =>
                {
                    options.UseInMemoryDatabase("InMemoryDbForTesting");
                });
            });
        }
    }
}