using System.Threading.Tasks;
using Microsoft.Extensions.Options;
using ToDo.API.Dto;
using ToDo.API.Helpers;
using ToDo.API.Wrappers;
using Google.Apis.Auth;

namespace ToDo.API.Services
{
    public class GoogleTokenService : IExternalTokenService
    {
        private readonly IGoogleJsonWebSignatureWrapper _googleJsonWebSignatureWrapper;
        private readonly GoogleAuthSettings _googleAuthSettings;

        public GoogleTokenService(
            IGoogleJsonWebSignatureWrapper googleJsonWebSignatureWrapper,
            IOptions<GoogleAuthSettings> googleAuthSettingsOptions)
        {
            _googleJsonWebSignatureWrapper = googleJsonWebSignatureWrapper;
            _googleAuthSettings = googleAuthSettingsOptions.Value;
        }

        public async Task<ExternalTokenPayload> ValidateAsync(string token)
        {
            GoogleJsonWebSignature.Payload googlePayload;

            var validationSettings = new GoogleJsonWebSignature.ValidationSettings
            {
                Audience = new[]
                {
                    _googleAuthSettings.ClientId
                }
            };

            try
            {
                googlePayload = await _googleJsonWebSignatureWrapper.ValidateAsync(token, validationSettings);
            }
            catch
            {
                return null;
            }

            return new ExternalTokenPayload
            {
                UserId = googlePayload.Subject,
                Email = googlePayload.Email,
                Username = googlePayload.Name,
                ProfilePictureUrl = googlePayload.Picture
            };
        }
    }
}