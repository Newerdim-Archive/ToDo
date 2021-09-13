using System.Threading.Tasks;
using Google.Apis.Auth;
using ToDo.API.Dto;
using ToDo.API.Enum;
using ToDo.API.Extensions;
using ToDo.API.Factories;

namespace ToDo.API.Services
{
    public class AuthService : IAuthService
    {
        private readonly IExternalTokenFactory _externalTokenFactory;
        private readonly IUserService _userService;

        public AuthService(IExternalTokenFactory externalTokenFactory, IUserService userService)
        {
            _externalTokenFactory = externalTokenFactory;
            _userService = userService;
        }
        
        public async Task<ExternalSignUpResult> ExternalSignUpAsync(string token, ExternalAuthProvider provider)
        {
            var tokenPayload = await _externalTokenFactory.ValidateAsync(token, provider);

            if (tokenPayload is null)
            {
                return new ExternalSignUpResult
                {
                    Message = ExternalSignUpResultMessage.TokenIsInvalid
                };
            }

            if (!tokenPayload.HasProfileInformation())
            {
                return new ExternalSignUpResult
                {
                    Message = ExternalSignUpResultMessage.TokenNotHaveProfileInformation
                };
            }

            var userExists = await _userService.ExistsByExternalIdAsync(tokenPayload.UserId, provider);

            if (userExists)
            {
                return new ExternalSignUpResult
                {
                    Message = ExternalSignUpResultMessage.UserAlreadyExists
                };
            }

            var createdUser = await _userService.CreateAsync(new CreateUser
            {
                ExternalId = tokenPayload.UserId,
                Email = tokenPayload.Email,
                Username = tokenPayload.Username,
                ProfilePictureUrl = tokenPayload.ProfilePictureUrl,
                Provider = provider
            });
            
            return new ExternalSignUpResult
            {
                Message = ExternalSignUpResultMessage.SignedUpSuccessfully,
                CreatedUser = createdUser
            };
        }
    }
}