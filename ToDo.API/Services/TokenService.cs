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
            var secret = _tokenSettings.AccessTokenSecret;
            
            var expiresTime = DateTime.UtcNow.AddMinutes(15);

            return CreateToken(userId, expiresTime, secret);
        }

        public string CreateRefreshToken(int userId)
        {
            var secret = _tokenSettings.RefreshTokenSecret;
            
            var expiresTime = DateTime.UtcNow.AddDays(14);

            return CreateToken(userId, expiresTime, secret);
        }

        /// <summary>
        /// Create Token
        /// </summary>
        /// <param name="userId">User id</param>
        /// <param name="expiresTime">Expires time</param>
        /// <param name="secret">Token secret</param>
        /// <returns>Token</returns>
        private static string CreateToken(int userId, DateTime expiresTime, string secret)
        {
            var claims = new Claim[]
            {
                new(ClaimTypes.NameIdentifier, userId.ToString())
            };

            var key = Encoding.UTF8.GetBytes(secret);

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(claims),
                Expires = expiresTime,
                SigningCredentials = new SigningCredentials(
                    new SymmetricSecurityKey(key),
                    SecurityAlgorithms.HmacSha512Signature)
            };
            
            var tokenHandler = new JwtSecurityTokenHandler();

            var securityToken = tokenHandler.CreateToken(tokenDescriptor);

            return tokenHandler.WriteToken(securityToken);
        }
    }
}