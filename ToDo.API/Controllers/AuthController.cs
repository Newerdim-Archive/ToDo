using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ToDo.API.Const;
using ToDo.API.Enum;
using ToDo.API.Helpers;
using ToDo.API.Models;
using ToDo.API.Services;

namespace ToDo.API.Controllers
{
    [Route("api/[controller]")]
    [Authorize]
    [ApiController]
    public class AuthController : CustomControllerBase
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

        [AllowAnonymous]
        [HttpPost("external-sign-up")]
        public async Task<IActionResult> ExternalSignUpAsync(ExternalSignUpModel model)
        {
            var externalAuthResult = await _authService.ExternalSignUpAsync(model.Token, model.Provider);

            switch (externalAuthResult.Message)
            {
                case ExternalSignUpResultMessage.SignedUpSuccessfully:
                    break;
                case ExternalSignUpResultMessage.TokenIsInvalid:
                    return BadRequest(nameof(model.Token), ValidationErrorMessage.IsInvalid);
                case ExternalSignUpResultMessage.TokenNotHaveProfileInformation:
                    return BadRequest(nameof(model.Token), ValidationErrorMessage.NotHaveProfileInformation);
                case ExternalSignUpResultMessage.UserAlreadyExists:
                    return Conflict(ResponseMessage.UserAlreadyExists);
                default:
                    throw new ArgumentOutOfRangeException(nameof(model), externalAuthResult.Message, null);
            }

            var refreshToken = _tokenService.CreateRefreshToken(externalAuthResult.CreatedUser.Id);

            _cookieService.Add(CookieName.RefreshToken, refreshToken);

            var accessToken = _tokenService.CreateAccessToken(externalAuthResult.CreatedUser.Id);

            return Ok(ResponseMessage.ActionPerformedSuccessfully, accessToken);
        }

        [AllowAnonymous]
        [HttpPost("log-out")]
        public IActionResult LogOut()
        {
            _cookieService.Delete(CookieName.RefreshToken);

            return Ok(ResponseMessage.ActionPerformedSuccessfully);
        }

        [AllowAnonymous]
        [HttpPost("external-log-in")]
        public async Task<IActionResult> ExternalLogInAsync(ExternalLogInModel model)
        {
            var externalLogInResult = await _authService.ExternalLogInAsync(model.Token, model.Provider);

            switch (externalLogInResult.Message)
            {
                case ExternalLogInResultMessage.LoggedInSuccessfully:
                    break;
                case ExternalLogInResultMessage.InvalidToken:
                    return BadRequest(nameof(model.Token), ValidationErrorMessage.IsInvalid);
                case ExternalLogInResultMessage.UserNotExist:
                    return Unauthorized(ResponseMessage.UserNotExist);
                default:
                    throw new ArgumentOutOfRangeException(nameof(model), externalLogInResult.Message, null);
            }

            var refreshToken = _tokenService.CreateRefreshToken(externalLogInResult.User.Id);

            _cookieService.Add(CookieName.RefreshToken, refreshToken);

            var accessToken = _tokenService.CreateAccessToken(externalLogInResult.User.Id);

            return Ok(ResponseMessage.ActionPerformedSuccessfully, accessToken);
        }

        [AllowAnonymous]
        [HttpGet("refresh-tokens")]
        public IActionResult RefreshTokens()
        {
            var refreshToken = _cookieService.GetValue(CookieName.RefreshToken);

            if (refreshToken is null)
            {
                return Unauthorized(ResponseMessage.RefreshTokenNotExist);
            }

            var userIdFromToken = _tokenService.GetUserIdFromRefreshToken(refreshToken);

            if (userIdFromToken is null)
            {
                return Unauthorized(ResponseMessage.RefreshTokenIsInvalid);
            }

            var newRefreshToken = _tokenService.CreateRefreshToken(userIdFromToken.Value);

            _cookieService.Add(CookieName.RefreshToken, newRefreshToken);

            var accessToken = _tokenService.CreateAccessToken(userIdFromToken.Value);

            return Ok(ResponseMessage.ActionPerformedSuccessfully, accessToken);
        }
    }
}