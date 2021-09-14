using System;
using System.Threading.Tasks;
using ToDo.API.Dto;
using ToDo.API.Enum;

namespace ToDo.API.Factories
{
    public interface IExternalTokenFactory
    {
        /// <summary>
        ///     Validate the token and return a payload
        /// </summary>
        /// <param name="token">Token</param>
        /// <param name="provider">Authentication provider</param>
        /// <returns>Payload if the token is valid; otherwise, null</returns>
        /// <exception cref="ArgumentOutOfRangeException">Throws when the provider is not implemented</exception>
        /// <exception cref="NullReferenceException">Throws when the service not exist</exception>
        public Task<ExternalTokenPayload> ValidateAsync(string token, ExternalAuthProvider provider);
    }
}