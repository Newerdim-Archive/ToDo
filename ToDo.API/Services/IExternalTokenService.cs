using System.Threading.Tasks;
using ToDo.API.Dto;

namespace ToDo.API.Services
{
    public interface IExternalTokenService
    {
        /// <summary>
        ///     Validate the token and return a payload
        /// </summary>
        /// <param name="token">Token</param>
        /// <returns>Payload if token is valid; otherwise, null</returns>
        public Task<ExternalTokenPayload> ValidateAsync(string token);
    }
}