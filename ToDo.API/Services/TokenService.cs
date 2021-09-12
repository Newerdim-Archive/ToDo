using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using ToDo.API.Helpers;

namespace ToDo.API.Services
{
    public class TokenService : ITokenService
    {
        private readonly TokenSettings _tokenSettings;

        public TokenService(IOptions<TokenSettings> tokenSettingsOptions)
        {
            _tokenSettings = tokenSettingsOptions.Value;
        }
        
        public string CreateAccessToken(int userId)
        {
            var key = Encoding.UTF8.GetBytes(_tokenSettings.AccessTokenSecret);
            
            var expiresTime = DateTime.UtcNow.AddMinutes(15);

            var claims = new Claim[]
            {
                new(ClaimTypes.NameIdentifier, userId.ToString())
            };

            var tokenHandler = new JwtSecurityTokenHandler();

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(claims),
                Expires = expiresTime,
                SigningCredentials = new SigningCredentials(
                    new SymmetricSecurityKey(key),
                    SecurityAlgorithms.HmacSha512Signature)
            };

            var securityToken = tokenHandler.CreateToken(tokenDescriptor);

            return tokenHandler.WriteToken(securityToken);
        }

        public string CreateRefreshToken(int userId)
        {
            var key = Encoding.UTF8.GetBytes(_tokenSettings.RefreshTokenSecret);
            
            var expiresTime = DateTime.UtcNow.AddDays(14);

            var claims = new Claim[]
            {
                new(ClaimTypes.NameIdentifier, userId.ToString())
            };

            var tokenHandler = new JwtSecurityTokenHandler();

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(claims),
                Expires = expiresTime,
                SigningCredentials = new SigningCredentials(
                    new SymmetricSecurityKey(key),
                    SecurityAlgorithms.HmacSha512Signature)
            };

            var securityToken = tokenHandler.CreateToken(tokenDescriptor);

            return tokenHandler.WriteToken(securityToken);
        }
    }
}