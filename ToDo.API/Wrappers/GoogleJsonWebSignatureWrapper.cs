using System.Threading.Tasks;
using Google.Apis.Auth;
using static Google.Apis.Auth.GoogleJsonWebSignature;

namespace ToDo.API.Wrappers
{
    public class GoogleJsonWebSignatureWrapper : IGoogleJsonWebSignatureWrapper
    {
        public async Task<Payload> ValidateAsync(string token, ValidationSettings validationSettings)
        {
            return await GoogleJsonWebSignature.ValidateAsync(token, validationSettings);
        }
    }
}