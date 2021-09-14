using System.Threading.Tasks;
using static Google.Apis.Auth.GoogleJsonWebSignature;

namespace ToDo.API.Wrappers
{
    public interface IGoogleJsonWebSignatureWrapper
    {
        /// <summary>
        ///     Validate token
        /// </summary>
        /// <param name="token">JWT</param>
        /// <param name="validationSettings">Validation settings</param>
        /// <returns>Token payload</returns>
        /// <exception cref="Google.Apis.Auth.InvalidJwtException">Throws when token is invalid</exception>
        Task<Payload> ValidateAsync(string token, ValidationSettings validationSettings);
    }
}