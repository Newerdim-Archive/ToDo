using System.Linq;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.DependencyInjection;
using ToDo.API.Wrappers;

namespace ToDo.IntegrationTests.Helpers
{
    public static class WebApplicationFactoryExtensions
    {
        public static WebApplicationFactory<TEntryPoint> ReplaceGoogleJsonWebSignatureWrapper<TEntryPoint>(
            this WebApplicationFactory<TEntryPoint> factory,
            IGoogleJsonWebSignatureWrapper googleJsonWebSignatureWrapper
        ) where TEntryPoint : class
        {
            return factory.WithWebHostBuilder(cfg =>
            {
                cfg.ConfigureServices(services =>
                {
                    var descriptor = services
                        .SingleOrDefault(d => d.ServiceType == typeof(IGoogleJsonWebSignatureWrapper));

                    services.Remove(descriptor);

                    services.AddTransient(_ => googleJsonWebSignatureWrapper);
                });
            });
        }
    }
}