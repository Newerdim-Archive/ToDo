using System.Threading.Tasks;
using ToDo.API.Dto;
using ToDo.API.Enum;

namespace ToDo.API.Services
{
    public interface IAuthService
    {
        /// <summary>
        ///     Sign up using external provider
        /// </summary>
        /// <param name="token">Token from external provider</param>
        /// <param name="provider">External Provider</param>
        /// <returns>
        ///     Result with message and user
        ///     <para>User is not null when signed up successfully</para>
        /// </returns>
        public Task<ExternalSignUpResult> ExternalSignUpAsync(string token, ExternalAuthProvider provider);
    }
}