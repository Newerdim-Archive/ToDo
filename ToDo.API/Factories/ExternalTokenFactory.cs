using System;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using ToDo.API.Dto;
using ToDo.API.Enum;
using ToDo.API.Services;

namespace ToDo.API.Factories
{
    public class ExternalTokenFactory : IExternalTokenFactory
    {
        private readonly IServiceProvider _serviceProvider;

        public ExternalTokenFactory(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }

        public async Task<ExternalTokenPayload> ValidateAsync(string token, ExternalAuthProvider provider)
        {
            var externalTokenService = GetService(provider);

            if (externalTokenService is null)
            {
                throw new NullReferenceException($"External token service with name {nameof(provider)} not exist");
            }

            return await externalTokenService.ValidateAsync(token);
        }

        private IExternalTokenService GetService(ExternalAuthProvider provider)
        {
            return provider switch
            {
                ExternalAuthProvider.Google => _serviceProvider.GetService<GoogleTokenService>(),
                _ => throw new ArgumentOutOfRangeException(nameof(provider), provider, null)
            };
        }
    }
}