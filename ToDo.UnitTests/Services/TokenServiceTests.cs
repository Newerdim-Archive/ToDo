using System;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using FluentAssertions;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Moq;
using ToDo.API.Helpers;
using ToDo.API.Services;
using Xunit;

namespace ToDo.UnitTests.Services
{
    public class TokenServiceTests
    {
        private readonly TokenService _sut;
        private readonly TokenSettings _tokenSettings;

        private readonly Mock<IOptions<TokenSettings>> _tokenSettingsOptionsMock = new();

        public TokenServiceTests()
        {
            _tokenSettings = new TokenSettings
            {
                AccessTokenSecret = "Test secret for access token",
                RefreshTokenSecret = "Test secret for refresh token"
            };

            _tokenSettingsOptionsMock
                .SetupGet(_ => _.Value)
                .Returns(_tokenSettings);

            _sut = new TokenService(_tokenSettingsOptionsMock.Object);
        }

        #region CreateAccessToken

        [Fact]
        public void CreateAccessToken_ReturnsToken()
        {
            // Arrange

            // Act
            var token = _sut.CreateAccessToken(1);

            // Assert
            token.Should().NotBeNullOrWhiteSpace();
        }

        [Fact]
        public void CreateAccessToken_TokenIsValidJwt()
        {
            // Arrange

            // Act
            var token = _sut.CreateAccessToken(1);

            var isTokenValid = IsTokenValid(token, _tokenSettings.AccessTokenSecret);

            // Assert
            isTokenValid.Should().BeTrue();
        }

        [Fact]
        public void CreateAccessToken_TokenContainsUserId()
        {
            // Arrange
            const int userId = 1;

            // Act
            var token = _sut.CreateAccessToken(userId);

            var userIdInToken = GetUserIdFromToken(token, _tokenSettings.AccessTokenSecret);

            // Assert
            userIdInToken.Should().NotBeNull().And.Be(userId);
        }

        [Fact]
        public void CreateAccessToken_TokenExpiresIn15Min()
        {
            // Arrange
            var expectedExpiresTime = DateTime.UtcNow.AddMinutes(15);

            // Act
            var token = _sut.CreateAccessToken(1);

            var expiresTime = GetExpiresTimeFromToken(token, _tokenSettings.AccessTokenSecret);

            // Assert
            expiresTime.Should().NotBeNull().And.BeCloseTo(expectedExpiresTime, TimeSpan.FromSeconds(5));

            _tokenSettingsOptionsMock.VerifyAll();
        }

        #endregion

        #region CreateRefreshToken

        [Fact]
        public void CreateRefreshToken_ReturnsToken()
        {
            // Arrange

            // Act
            var token = _sut.CreateRefreshToken(1);

            // Assert
            token.Should().NotBeNullOrWhiteSpace();
        }

        [Fact]
        public void CreateRefreshToken_TokenIsValidJwt()
        {
            // Arrange

            // Act
            var token = _sut.CreateRefreshToken(1);

            var isTokenValid = IsTokenValid(token, _tokenSettings.RefreshTokenSecret);

            // Assert
            isTokenValid.Should().BeTrue();
        }

        [Fact]
        public void CreateRefreshToken_TokenContainsUserId()
        {
            // Arrange
            const int userId = 1;

            // Act
            var token = _sut.CreateRefreshToken(userId);

            var userIdInToken = GetUserIdFromToken(token, _tokenSettings.RefreshTokenSecret);

            // Assert
            userIdInToken.Should().Be(userId);
        }

        [Fact]
        public void CreateRefreshToken_TokenExpiresIn14Days()
        {
            // Arrange
            var expectedExpiresTime = DateTime.UtcNow.AddDays(14);

            // Act
            var token = _sut.CreateRefreshToken(1);

            var expiresTime = GetExpiresTimeFromToken(token, _tokenSettings.RefreshTokenSecret);

            // Assert
            expiresTime.Should().BeCloseTo(expectedExpiresTime, TimeSpan.FromSeconds(5));
        }

        #endregion

        /// <summary>
        /// Get user id from token
        /// </summary>
        /// <param name="token">JWT</param>
        /// <param name="secret">Token secret</param>
        /// <returns>User id if token and user id is valid; otherwise, null</returns>
        private static int? GetUserIdFromToken(string token, string secret)
        {
            var result = ValidateToken(token, secret);

            if (result is null)
            {
                return null;
            }

            var (principal, _) = result.Value;
            
            try
            {
                var userId = principal.Claims.First(x => x.Type == ClaimTypes.NameIdentifier).Value;

                return Convert.ToInt32(userId);
            }
            catch
            {
                return null;
            }
        }

        /// <summary>
        /// Get expires time from token
        /// </summary>
        /// <param name="token">JWT</param>
        /// <param name="secret">Token secret</param>
        /// <returns>Expires time if token is valid; otherwise, null</returns>
        private static DateTime? GetExpiresTimeFromToken(string token, string secret)
        {
            var result = ValidateToken(token, secret);

            if (result is null)
            {
                return null;
            }

            var (_, secureToken) = result.Value;

            return secureToken.ValidTo;
        }

        /// <summary>
        /// Validate JWT token
        /// </summary>
        /// <param name="token">JWT</param>
        /// <param name="secret">Token secret</param>
        /// <returns>Tuple if token is valid; otherwise, null</returns>
        private static (ClaimsPrincipal, SecurityToken)? ValidateToken(string token, string secret)
        {
            var key = Encoding.UTF8.GetBytes(secret);

            var tokenHandler = new JwtSecurityTokenHandler();

            var validationParameters = GetTokenValidationParameters(key);

            try
            {
                var principal = tokenHandler.ValidateToken(token, validationParameters, out var securityToken);

                return (principal, securityToken);
            }
            catch
            {
                return null;
            }
        }

        /// <summary>
        /// Check if token is valid using secret
        /// </summary>
        /// <param name="token">JWT</param>
        /// <param name="secret">Token secret</param>
        /// <returns>True if is valid; otherwise, null</returns>
        private static bool IsTokenValid(string token, string secret)
        {
            var result = ValidateToken(token, secret);
            
            return result is not null;
        }
        
        /// <summary>
        /// Get token validation parameters
        /// </summary>
        /// <param name="key">Token secret key</param>
        /// <returns>Token validation parameters</returns>
        private static TokenValidationParameters GetTokenValidationParameters(byte[] key)
        {
            return new TokenValidationParameters()
            {
                ValidateLifetime = false,
                ValidateAudience = false,
                ValidateIssuer = false,
                IssuerSigningKey = new SymmetricSecurityKey(key)
            };
        }
    }
}