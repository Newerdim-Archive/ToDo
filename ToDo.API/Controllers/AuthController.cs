using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using ToDo.API.Const;
using ToDo.API.Dto;
using ToDo.API.Enum;
using ToDo.API.Models;
using ToDo.API.Services;

namespace ToDo.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authService;
        private readonly ICookieService _cookieService;
        private readonly ITokenService _tokenService;

        public AuthController(ITokenService tokenService, ICookieService cookieService, IAuthService authService)
        {
            _tokenService = tokenService;
            _cookieService = cookieService;
            _authService = authService;
        }

        [HttpPost("external-sign-up")]
        public async Task<IActionResult> ExternalSignUpAsync(ExternalSignUpModel model)
        {
            var externalAuthResult = await _authService.ExternalSignUpAsync(model.Token, model.Provider);

            switch (externalAuthResult.Message)
            {
                case ExternalSignUpResultMessage.SignedUpSuccessfully:
                    break;
                case ExternalSignUpResultMessage.TokenIsInvalid:
                    ModelState.AddModelError(nameof(model.Token), ResponseMessage.TokenIsInvalid);
                    return BadRequest(ModelState);
                case ExternalSignUpResultMessage.TokenNotHaveProfileInformation:
                    ModelState.AddModelError(nameof(model.Token), ResponseMessage.TokenNotHaveProfileInformation);
                    return BadRequest(ModelState);
                case ExternalSignUpResultMessage.UserAlreadyExists:
                    return Conflict(ResponseMessage.UserAlreadyExists);
                default:
                    throw new ArgumentOutOfRangeException(nameof(model), externalAuthResult.Message, null);
            }

            var refreshToken = _tokenService.CreateRefreshToken(externalAuthResult.CreatedUser.Id);

            _cookieService.Add(CookieName.RefreshToken, refreshToken);

            var accessToken = _tokenService.CreateAccessToken(externalAuthResult.CreatedUser.Id);

            var response = new ExternalSignUpResponse
            {
                Message = ResponseMessage.SignedUpSuccessfully,
                AccessToken = accessToken
            };

            return Ok(response);
        }
    }
}