using System.Threading.Tasks;
using Google.Apis.Auth;

namespace ToDo.API.Wrappers
{
    public class GoogleJsonWebSignatureWrapper : IGoogleJsonWebSignatureWrapper
    {
        public async Task<GoogleJsonWebSignature.Payload> ValidateAsync(string token, GoogleJsonWebSignature.ValidationSettings validationSettings)
        {
            return await GoogleJsonWebSignature.ValidateAsync(token, validationSettings);
        }
    }
}